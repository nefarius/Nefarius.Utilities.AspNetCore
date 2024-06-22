# W3CLoggingOptions

Namespace: Nefarius.Utilities.AspNetCore.Options

W3C Logging options.

```csharp
public sealed class W3CLoggingOptions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [W3CLoggingOptions](./nefarius.utilities.aspnetcore.options.w3cloggingoptions.md)

## Properties

### <a id="properties-compressdeletedlogfiles"/>**CompressDeletedLogFiles**

If set, will create a compressed archive copy of a rolled log file and deletes the original afterwards. If false,
 the log processor will retain its default behaviour (just deletes older files if
 [W3CLoggingOptions.RetainedFileCountLimit](./nefarius.utilities.aspnetcore.options.w3cloggingoptions.md#retainedfilecountlimit) is hit). Defaults to true.

```csharp
public bool CompressDeletedLogFiles { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-compressedlogsdirectory"/>**CompressedLogsDirectory**

Absolute path to directory where compressed/archived logs will get stored. Defaults to "logs/archived"
 sub-directory within the application root path.

```csharp
public string CompressedLogsDirectory { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

**Remarks:**

You MUST NOT set [W3CLoggingOptions.LogsDirectory](./nefarius.utilities.aspnetcore.options.w3cloggingoptions.md#logsdirectory) to the same location, or you risk other log processors
 within ASP.NET Core to accidentally delete your archived logs.

### <a id="properties-filename"/>**FileName**

Log file prefix name. Defaults to "access-".

```csharp
public string FileName { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-filesizelimit"/>**FileSizeLimit**

Maximum log file size in bytes or null for no limit. Defaults to 100 MB.

```csharp
public Nullable<Int32> FileSizeLimit { get; set; }
```

#### Property Value

[Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

### <a id="properties-flushinterval"/>**FlushInterval**

Period after which the contents will get flushed to the log file. Defaults to 2 seconds.

```csharp
public TimeSpan FlushInterval { get; set; }
```

#### Property Value

[TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>

### <a id="properties-loggingfields"/>**LoggingFields**

Fields to include in log. Defaults to all of them.

```csharp
public W3CLoggingFields LoggingFields { get; set; }
```

#### Property Value

W3CLoggingFields<br>

### <a id="properties-logsdirectory"/>**LogsDirectory**

Absolute path to directory where logs will get stored. Defaults to "logs" sub-directory within the application root
 path.

```csharp
public string LogsDirectory { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

**Remarks:**

You MUST NOT set [W3CLoggingOptions.CompressedLogsDirectory](./nefarius.utilities.aspnetcore.options.w3cloggingoptions.md#compressedlogsdirectory) to the same location, or you risk other log processors
 within ASP.NET Core to accidentally delete your archived logs.

### <a id="properties-retainedcompressedfilecountlimit"/>**RetainedCompressedFileCountLimit**

Maximum number of compressed files to retain. Set to zero to not delete any. Does nothing if
 [W3CLoggingOptions.CompressDeletedLogFiles](./nefarius.utilities.aspnetcore.options.w3cloggingoptions.md#compressdeletedlogfiles) is false. Defaults to 90.

```csharp
public int RetainedCompressedFileCountLimit { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-retainedfilecountlimit"/>**RetainedFileCountLimit**

Maximum number of plain-text files to retain. See [W3CLoggingOptions.CompressDeletedLogFiles](./nefarius.utilities.aspnetcore.options.w3cloggingoptions.md#compressdeletedlogfiles) and
 [W3CLoggingOptions.RetainedCompressedFileCountLimit](./nefarius.utilities.aspnetcore.options.w3cloggingoptions.md#retainedcompressedfilecountlimit) to influence log compression and archived files retention. Defaults
 to 3.

```csharp
public int RetainedFileCountLimit { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
