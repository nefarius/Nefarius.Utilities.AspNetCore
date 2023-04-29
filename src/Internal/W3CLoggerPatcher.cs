using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using MonoMod.RuntimeDetour;

namespace Nefarius.Utilities.AspNetCore.Internal;

/// <summary>
///     Alters the file rolling behaviour of the W3CLoggerProcessor.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
internal sealed class W3CLoggerPatcher
{
    private static IDetour _hookRollFiles;
    private static readonly Type FileLoggerProcessor;

    static W3CLoggerPatcher()
    {
        FileLoggerProcessor = GetTypeByName("Microsoft.AspNetCore.HttpLogging.FileLoggerProcessor");
    }

    /// <summary>
    ///     Patch desired methods.
    /// </summary>
    public static void Patch()
    {
        _hookRollFiles = new Hook(
            FileLoggerProcessor.GetMethod("RollFiles", BindingFlags.Instance | BindingFlags.NonPublic),
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

    private static object GetInstanceField(Type type, object instance, string fieldName)
    {
        BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                 | BindingFlags.Static;
        FieldInfo field = type.GetField(fieldName, bindFlags);
        return field?.GetValue(instance);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private static void RollFiles(dynamic fileLoggerProcessor)
    {
        /*
         * this might look like a terrible idea and quite slow but this call happens
         * on a background thread and does file I/O which itself can be rather slow
         * so we're all good :)
         */
        
        string path = GetInstanceField(FileLoggerProcessor, fileLoggerProcessor, "_path");
        string fileName = GetInstanceField(FileLoggerProcessor, fileLoggerProcessor, "_fileName");
        object pathLock = GetInstanceField(FileLoggerProcessor, fileLoggerProcessor, "_pathLock");
        int? maxRetainedFiles = GetInstanceField(FileLoggerProcessor, fileLoggerProcessor, "_maxRetainedFiles");

        // ...
    }
}