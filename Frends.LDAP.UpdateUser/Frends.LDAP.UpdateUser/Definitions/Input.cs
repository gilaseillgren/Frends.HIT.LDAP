using System;
using System.ComponentModel;

namespace Frends.LDAP.UpdateUser.Definitions;

/// <summary>
/// Input parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// Modification method.
    /// </summary>
    /// <example>ModificationMethod.Add</example>
    [DefaultValue(ModificationMethod.Add)]
    public ModificationMethod ModificationMethod { get; set; }

    /// <summary>
    /// Common name (CN).
    /// </summary>
    /// <example>Firstname Lastname</example>
    public string CommonName { get; set; }

    /// <summary>
    /// Directory path.
    /// </summary>
    /// <example>CN=Users,DC=Example,DC=Com</example>
    [DefaultValue("CN=Users,DC=Example,DC=Com")]
    public string Path { get; set; }

    /// <summary>
    /// Attributes to be updated. 
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