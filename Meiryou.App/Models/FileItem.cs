namespace Meiryou.Models;

/// <summary>
/// A file to display in a list.
/// </summary>
/// <param name="fileName">The displau name of the file.</param>
/// <param name="filePath">The full path of the file.</param>
public class FileItem(string fileName, string filePath)
{
    /// <summary>
    /// Initialises a new instance of the FileItem class.
    /// </summary>
    public FileItem() : this(string.Empty, string.Empty) {}

    public string FileName { get; set; } = fileName;
    public string FilePath { get; set; } = filePath;
}