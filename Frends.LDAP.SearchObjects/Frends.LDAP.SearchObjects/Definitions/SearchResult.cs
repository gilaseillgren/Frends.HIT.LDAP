using System.Collections.Generic;
namespace Frends.LDAP.SearchObjects.Definitions;

/// <summary>
/// Search result.
/// </summary>
public class SearchResult
{
    /// <summary>
    /// Distinguished name of the entry.
    /// </summary>
    public string DistinguishedName { get; set; }

    /// <summary>
    /// Search result's attributes.
    /// </summary>
    public List<AttributeSet> AttributeSet { get; set; }
}

/// <summary>
/// AttributeSet values.
/// </summary>
public class AttributeSet
{
    /// <summary>
    /// Key.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Value.
    /// </summary>
    public string Value { get; set; }
}