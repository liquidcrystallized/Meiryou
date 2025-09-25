using System.Collections.Generic;

namespace Meiryou.Services;

/// <summary>
/// Manages the file system paths used by the application.
/// </summary>
public interface IAppPathService
{
    /// <summary>
    /// Returns all valid text file extensions.
    /// </summary>
    IReadOnlyList<string> TextExtensions { get; }
}