using System.Collections.Generic;
using Meiryou.Models;

namespace Meiryou.Business;

/// <summary>
/// Provides discovery service for files.
/// </summary>
public interface IFileLocator
{
    /// <summary>
    /// Returns a list of all text files in a specified directory, searching recursively.
    /// </summary>
    /// <param name="filePath">The path to search for text files.</param>
    /// <returns>A list of text files.</returns>
    IEnumerable<FileItem> GetTextFiles(string filePath);
   
    /// <summary>
    /// Returns a list of all text files in multiple specified directories, searching recursively.
    /// </summary>
    /// <param name="filePaths">A list of paths to search for text files.</param>
    /// <returns>A list of text files.</returns>
    IEnumerable<FileItem> GetTextFiles(IEnumerable<string> filePaths);
}