using System.Collections.Generic;
namespace Frends.LDAP.SearchObjects.Definitions;

/// <summary>
/// Task's result.
/// </summary>
public class Result
{
    /// <summary>
    /// Search completed.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; private set; }

    /// <summary>
    /// LDAP Error message.
    /// </summary>
    /// <example>Entry Already Exists</example>
    public string Error { get; private set; }

    /// <summary>
    /// Results.
    /// </summary>
    public List<SearchResult> SearchResult { get; private set; }

    internal Result(bool success, string error, List<SearchResult> searchResult)
    {
        Success = success;
        Error = error;
        SearchResult = searchResult;
    }
}