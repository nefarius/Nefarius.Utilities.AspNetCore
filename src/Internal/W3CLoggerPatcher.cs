using System;
using System.Linq;
using System.Runtime.CompilerServices;

using HarmonyLib;

namespace Nefarius.Utilities.AspNetCore.Internal;

internal sealed class W3CLoggerPatcher
{
    public static void Patch()
    {
        Harmony.DEBUG = true;
        
        var harmony = new Harmony("Microsoft.AspNetCore.HttpLogging.FileLoggerProcessor");
        
        var fileLoggerProcessor = ByName("Microsoft.AspNetCore.HttpLogging.FileLoggerProcessor");
        var original = AccessTools.Method(fileLoggerProcessor, "RollFiles");
        var prefix = typeof(W3CLoggerPatcher).GetMethod("RollFilesPrefix");
        var postfix = typeof(W3CLoggerPatcher).GetMethod("RollFilesPostfix");
        
        harmony.Patch(original, new HarmonyMethod(prefix), new HarmonyMethod(postfix));
    }
    
    private static Type ByName(string name)
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

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void RollFilesPrefix()
    {
        // ...
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void RollFilesPostfix()
    {
        // ...
    }
}