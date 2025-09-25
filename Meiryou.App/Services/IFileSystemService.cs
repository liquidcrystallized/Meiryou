using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace Meiryou.Services;

/// <summary>
/// Extends IFileSystem with a few extra IO functions. IFileSystem provides wrappers around all IO methods.
/// </summary>
public interface IFileSystemService : IFileSystem
{
    /// <summary>
    /// Returns all files of specified extensions.
    /// </summary>
    /// <param name="path">The path in which to search.</param>
    /// <param name="extensions">A list of file extensions to return, each extension must include the dot.</param>
    /// <param name="searchOption">Specifies additional search options.</param>
    /// <returns>A list of files paths matchin search conditions.</returns>
    IEnumerable<string> GetFilesByExtensions(string path, IEnumerable<string> extensions, SearchOption searchOption = SearchOption.TopDirectoryOnly);
   
    /// <summary>
    /// Returns the path ensuring it ends with a directory separator char.
    /// </summary>
    /// <param name="path">The path to end with a separator char.</param>
    /// <returns>A path that must end with a directory separator char.</returns>
    string GetPathWithFinalSeparator(string path);
}