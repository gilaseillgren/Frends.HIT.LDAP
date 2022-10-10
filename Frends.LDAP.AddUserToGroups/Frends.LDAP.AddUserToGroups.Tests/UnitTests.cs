using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.LDAP.AddUserToGroups.Definitions;
using Novell.Directory.Ldap;
namespace Frends.LDAP.AddUserToGroups.Tests;

[TestClass]
public class UnitTests
{
    /*
        LDAP server to docker.
        docker run -d -it --rm -p 10389:10389 dwimberger/ldap-ad-it
    */
    private readonly string? _host = "127.0.0.1";
    private readonly int _port = 10389;
    private readonly string? _user = "uid=admin,ou=system";
    private readonly string? _pw = "secret";
    private readonly string _path = "ou=users,dc=wimpi,dc=net";
    private readonly string? _groupDn = "cn=admin,ou=roles,dc=wimpi,dc=net";

    Input? input;
    Connection? connection;

    [TestMethod]
    public void Update_HandleLDAPError_Test()
    {
        input = new()
        {
            UserDistinguishedName = "CN=Common Name,CN=Users,DC=Example,DC=Com",
            GroupDistinguishedName = "CN=Admins,DC=Example,DC=Com"
        };
        connection = new()
        {
            Host = _host,
            User = _user,
            Password = _pw,
            SecureSocketLayer = false,
            Port = _port,
            TLS = false,
        };

        var result = LDAP.AddUserToGroups(input, connection);
        Assert.IsTrue(result.Success.Equals(false) && result.Error.Contains("No Such Object"));
    }

    [TestMethod]
    public void AddUserToGroups_Test()
    {
        var tuser = "Tes Tuser" + Guid.NewGuid().ToString();
        var dn = $"CN={tuser},ou=users,dc=wimpi,dc=net";
        CreateTestUsers(tuser);

        input = new()
        {
            UserDistinguishedName = dn,
            GroupDistinguishedName = _groupDn
        };
        connection = new()
        {
            Host = _host,
            User = _user,
            Password = _pw,
            SecureSocketLayer = false,
            Port = _port,
            TLS = false,
        };

        var result = LDAP.AddUserToGroups(input, connection);
        Assert.IsTrue(result.Success.Equals(true));
    }

    public void CreateTestUsers(string tuser)
    {
        try
        {
            LdapConnection conn = new();
            conn.Connect(_host, _port);
            conn.Bind(_user, _pw);

            var attributeSet = new LdapAttributeSet();
            attributeSet.Add(new LdapAttribute("objectclass", "user"));
            attributeSet.Add(new LdapAttribute("cn", tuser));
            attributeSet.Add(new LdapAttribute("givenname", "Tes"));
            attributeSet.Add(new LdapAttribute("sn", tuser.Split(' ', 1)));

            var entry = $"CN={tuser},{_path}";
            LdapEntry newEntry = new(entry, attributeSet);
            conn.Add(newEntry);
            conn.Disconnect();
        }
        catch (Exception)
        {
        }
    }
}