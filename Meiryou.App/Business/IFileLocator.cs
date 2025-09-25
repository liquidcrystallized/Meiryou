using System.Collections.Generic;
using Meiryou.Models;

namespace Meiryou.Business;

/// <summary>
/// Provides discovery service for files.
/// </summary>
public interface IFileLocator
{
    /// <summary>
    /// Returns a list of all files in a specified directory, searching recursively.
    /// </summary>
    /// <param name="filePath">The path to search for files.</param>
    /// <returns>A list of files.</returns>
    IEnumerable<FileItem> GetFiles(string filePath);
   
    /// <summary>
    /// Returns a list of all files in multiple specified directories, searching recursively.
    /// </summary>
    /// <param name="filePaths">A list of paths to search for files.</param>
    /// <returns>A list of files.</returns>
    IEnumerable<FileItem> GetFiles(IEnumerable<string> filePaths);
}