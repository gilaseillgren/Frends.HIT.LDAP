using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.LDAP.CreateUser.Definitions;
using Novell.Directory.Ldap;
namespace Frends.LDAP.CreateUser.Tests;

[TestClass]
public class UnitTests
{
    private readonly string? _host = Environment.GetEnvironmentVariable("HiQ_AzureADTest_Address");
    private readonly string? _user = Environment.GetEnvironmentVariable("HiQ_AzureADTest_User");
    private readonly string? _pw = Environment.GetEnvironmentVariable("HiQ_AzureADTest_Password");

    Input? input;
    Connection? connection;

    [TestMethod]
    public void Create_HandleLDAPError_Test()
    {
        input = new()
        {
            Path = "CN=Users,DC=FRENDSTest01,DC=net",
            CommonName = "MattiMeikalainen_"+new Guid(),
            ObjectClass = "inetOrgPerson", //This will cause an error.
            GivenName = "Matti",
            Surname = "Meikäläinen",
            Email = "MattiMeikalainen@foo.bar",
            SetPassword = true,
            Password = "Qwerty123!",
        };
        connection = new()
        {
            Host = _host,
            User = _user,
            Password = _pw,
            SecureSocketLayer = false,
            Port = LdapConnection.DefaultPort,
            TLS = false,
        };

        var result = LDAP.CreateUser(input, connection);
        Assert.IsTrue(result.Success.Equals(false) && result.Error.Contains("Insufficient Access Rights"));
    }

    [TestMethod]
    public void Create_Test()
    {
        var attributes = new List<Attributes> {
            new Attributes() { Key = "telephoneNumber", Value = "+123123123"},
        };

        input = new()
        {
            Path = "CN=Users,DC=FRENDSTest01,DC=net",
            CommonName = "MattiMeikalainen",
            ObjectClass = "user",
            GivenName = "Matti",
            Surname = "Meikäläinen",
            Email = "MattiMeikalainen@foo.bar",
            SetPassword = true,
            Password = "Qwerty123!",
            Attributes = attributes.ToArray()
        };
        connection = new()
        {
            Host = _host,
            User = _user,
            Password = _pw,
            SecureSocketLayer = false,
            Port = LdapConnection.DefaultPort,
            TLS = false,
        };

        var result = LDAP.CreateUser(input, connection);
        Assert.IsTrue(result.Success.Equals(true));
        DeleteUser();
    }

    public void DeleteUser() 
    {
        LdapConnection ldapConn = new();
        ldapConn.Connect(_host, 389);
        ldapConn.Bind(_user, _pw);
        ldapConn.Delete("CN=MattiMeikalainen;CN=Users,DC=FRENDSTest01,DC=net");
    }
}