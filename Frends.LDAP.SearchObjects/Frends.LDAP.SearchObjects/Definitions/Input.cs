using System;
using System.ComponentModel;

namespace Frends.LDAP.SearchObjects.Definitions;

/// <summary>
/// Input parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// The search base parameter specifies the DN of the entry where you want to begin the search, such as ou=development, o=acme.
    /// If you want the search to begin at the tree root pass an empty string.
    /// </summary>
    public string SearchBase { get; set; }

    /// <summary>
    /// The search scope parameter specifies the depth of the search and can be one of three values:
    /// ScopeBase - Only the entry specified as the search base is included in the search. This is used when you already know the DN of the object and you want to read its attributes. The read method may also be used to read the values of a single entry.
    /// ScopeOne - Objects one level below the base (but not including the base) are included in the search. If we specified o=acme as our search base, then entries in the o=acme container would be included, but not the object o=acme.
    /// ScopeSub - All objects below the base, including the base itself, are included in the search.
    /// </summary>
    /// <example>Scopes.ScopeBase</example>
    public Scopes Scope { get; set; }

    /// <summary>
    /// The search filter defines the entries that will be returned by the search. The LDAP search filter grammar is specified in RFC 2254 and 2251. The grammar uses ABNF notation. If you are looking for all employees with a title of engineer, the search filter would be (title=engineer). 
    /// </summary>
    /// <example>title=engineer</example>
    public string Filter { get; set; }

    /// <summary>
    /// The maximum time in milliseconds to wait for results. The default is 0, which means that there is no maximum time limit.
    /// </summary>
    /// <example>0</example>
    [DefaultValue(0)]
    public int MsLimit { get; set; }

    /// <summary>
    /// The maximum time in seconds that the server should spend returning search results. This is a server-enforced limit. The default of 0 means no time limit.
    /// </summary>
    /// <example>0</example>
    [DefaultValue(0)]
    public int ServerTimeLimit { get; set; }

    /// <summary>
    /// Specifies when aliases should be dereferenced.
    /// DerefNever: Indicates that aliases are never dereferenced.
    /// DerefSearching: Indicates that aliases are are derefrenced when searching the entries beneath the starting point of the search, but not when finding the starting entry.
    /// DerefFinding: Indicates that aliases are dereferenced when finding the starting point for the search, but not when searching under that starting entry.
    /// DerefAlways: Indicates that aliases are always dereferenced, both when finding the starting point for the search, and also when searching the entries beneath the starting entry.
    /// </summary>
    /// <example>SearchConstraints.DerefNever</example>
    [DefaultValue(SearchDereference.DerefNever)]
    public SearchDereference SearchDereference { get; set; }

    /// <summary>
    /// The maximum number of search results to return for a search request.
    /// </summary>
    /// <example>1000</example>
    [DefaultValue(1000)]
    public int MaxResults { get; set; }

    /// <summary>
    /// The number of results to return in a batch. Specifying 0 means to block until all results are received. Specifying 1 means to return results one result at a time.
    /// </summary>
    /// <example>1</example>
    [DefaultValue(1)]
    public int BatchSize { get; set; }

    /// <summary>
    /// If true, returns the names but not the values of the attributes found. 
    /// If false, returns the names and values for attributes found.
    /// </summary>
    /// <example>false</example>
    [DefaultValue(false)]
    public bool TypesOnly { get; set; }

    /// <summary>
    /// Operational attributes are not automatically returned in search results; they must be requested by name in the search operation. For a list of supported operational attributes in Novell eDirectory, see https://www.novell.com/documentation/developer/ldapover/ldap_enu/data/a3xfo7x.html
    /// </summary>
    /// <example>Telephone, +358123456789</example>
    public Attributes[] Attributes { get; set; } = Array.Empty<Attributes>();
}

/// <summary>
/// Attribute values.
/// </summary>
public class Attributes
{
    /// <summary>
    /// Key.
    /// </summary>
    /// <example>Telephone</example>
    public string Key { get; set; }

    /// <summary>
    /// Value.
    /// </summary>
    /// <example>+358123456789</example>
    public string Value { get; set; }
}