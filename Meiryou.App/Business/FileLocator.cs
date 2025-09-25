using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Meiryou.Models;
using Meiryou.Services;

namespace Meiryou.Business;

/// <summary>
/// Provides discover service for (mostly) text files.
/// </summary>
public class FileLocator : IFileLocator
{
    private readonly IAppPathService _appPath;
    private readonly IFileSystemService _fileSystem;

    public FileLocator(IAppPathService appPathService, IFileSystemService fileSystemService)
    {
        _appPath = appPathService;
        _fileSystem = fileSystemService;
    }
    
    public IEnumerable<FileItem> GetTextFiles(string path)
    {
        path = _fileSystem.GetPathWithFinalSeparator(path);
        return _fileSystem.GetFilesByExtensions(path, _appPath.TextExtensions, SearchOption.AllDirectories).Select(x =>
            new FileItem(x, TrimPath(x, path)));
    }

    public IEnumerable<FileItem> GetTextFiles(IEnumerable<string> paths)
    {
        ArgumentNullException.ThrowIfNull(paths);
        
        var result = new List<FileItem>();
        foreach (var item in paths)
        {
            result.AddRange(GetTextFiles(item));
        }

        return result;
    }
  
    // Remove load path from file path.
    private string TrimPath(string file, string path) =>
        file.StartsWith(path) ? file[path.Length..] : file;
}