using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

using MonoMod.RuntimeDetour;

namespace Nefarius.Utilities.AspNetCore.Internal;

/// <summary>
///     Alters the file rolling behaviour of the W3CLoggerProcessor.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
internal sealed class W3CLoggerPatcher
{
    private static IDetour _hookRollFiles;
    private static readonly Type FileLoggerProcessorType;

    static W3CLoggerPatcher()
    {
        FileLoggerProcessorType = GetTypeByName("Microsoft.AspNetCore.HttpLogging.FileLoggerProcessor");
    }

    internal static int RetainedCompressedFileCountLimit { get; set; }

    /// <summary>
    ///     Patch desired methods.
    /// </summary>
    public static void Patch()
    {
        _hookRollFiles = new Hook(
            FileLoggerProcessorType.GetMethod("RollFiles", BindingFlags.Instance | BindingFlags.NonPublic),
            typeof(W3CLoggerPatcher).GetMethod("RollFiles", BindingFlags.Static | BindingFlags.NonPublic)
        );
    }

    /// <summary>
    ///     Restore patches to their original states.
    /// </summary>
    public static void Unpatch()
    {
        _hookRollFiles.Undo();
    }

    /// <summary>
    ///     Hooks W3CLoggerProcessor.RollFiles.
    /// </summary>
    /// <param name="fileLoggerProcessor">The hooked instance.</param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private static void RollFiles(dynamic fileLoggerProcessor)
    {
        /*
         * this might look like a terrible idea and quite slow but this call happens
         * on a background thread and does file I/O which itself can be rather slow
         * so we're all good :)
         */

        Type type = fileLoggerProcessor.GetType();
        string path = GetInstanceField(FileLoggerProcessorType, fileLoggerProcessor, "_path");
        string fileName = GetInstanceField(FileLoggerProcessorType, fileLoggerProcessor, "_fileName");
        object pathLock = GetInstanceField(FileLoggerProcessorType, fileLoggerProcessor, "_pathLock");
        int? maxRetainedFiles = GetInstanceField(FileLoggerProcessorType, fileLoggerProcessor, "_maxRetainedFiles");

        if (!(maxRetainedFiles > 0))
        {
            return;
        }

        lock (pathLock)
        {
            IEnumerable<FileInfo> sourceFiles = new DirectoryInfo(path)
                .GetFiles(fileName + "*.txt")
                .OrderByDescending(f => f.Name)
                .Skip(maxRetainedFiles.Value);

            foreach (FileInfo originalFile in sourceFiles)
            {
                // do not alter default behaviour for other processors
                if (type.FullName == "Microsoft.AspNetCore.HttpLogging.W3CLoggerProcessor")
                {
                    string archiveFileName = $"{originalFile.Name}.tar.gz";
                    string archiveFilePath = Path.Combine(path, archiveFileName);

                    using FileStream outStream = File.Create(archiveFilePath);
                    using GZipOutputStream gzoStream = new GZipOutputStream(outStream);
                    using TarArchive tarArchive = TarArchive.CreateOutputTarArchive(gzoStream);

                    tarArchive.RootPath = "/";

                    TarEntry tarEntry = TarEntry.CreateEntryFromFile(originalFile.FullName);
                    tarEntry.Name = Path.GetFileName(originalFile.FullName);

                    tarArchive.WriteEntry(tarEntry, true);
                }

                originalFile.Delete();
            }

            if (RetainedCompressedFileCountLimit <= 0)
            {
                return;
            }

            IEnumerable<FileInfo> archivedFiles = new DirectoryInfo(path)
                .GetFiles(fileName + "*.tar.gz")
                .OrderByDescending(f => f.Name)
                .Skip(RetainedCompressedFileCountLimit);

            foreach (FileInfo archivedFile in archivedFiles)
            {
                archivedFile.Delete();
            }
        }
    }

    /// <summary>
    ///     Find a type by fully qualified name in all assemblies.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The type.</returns>
    private static Type GetTypeByName(string name)
    {
        return
            AppDomain.CurrentDomain.GetAssemblies()
                .Reverse()
                .Select(assembly => assembly.GetType(name))
                .FirstOrDefault(t => t != null)
            // Safely delete the following part
            // if you do not want fall back to first partial result
            ??
            AppDomain.CurrentDomain.GetAssemblies()
                .Reverse()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(t => t.Name.Contains(name));
    }

    /// <summary>
    ///     Gets a field value via reflection.
    /// </summary>
    /// <param name="type">The instance type.</param>
    /// <param name="instance">The instance object to read from.</param>
    /// <param name="fieldName">The name of the field.</param>
    /// <returns>The field value.</returns>
    private static object GetInstanceField(Type type, object instance, string fieldName)
    {
        BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                 | BindingFlags.Static;
        FieldInfo field = type.GetField(fieldName, bindFlags);
        return field?.GetValue(instance);
    }
}