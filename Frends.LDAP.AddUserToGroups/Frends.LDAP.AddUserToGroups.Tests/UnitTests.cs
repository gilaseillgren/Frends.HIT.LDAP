using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.LDAP.AddUserToGroups.Definitions;
using Novell.Directory.Ldap;
namespace Frends.LDAP.AddUserToGroups.Tests;

[TestClass]
public class UnitTests
{
    // Azure AD.
    /*
        private readonly int _port = 389;
        private readonly string? _path = "CN=Users,DC=FRENDSTest01,DC=net";
        private readonly string? _dn = "CN=Tes Tuser,CN=Users,DC=FRENDSTest01,DC=net";
        private readonly string? _groupDn = "CN=Users,CN=Builtin,DC=FRENDSTest01,DC=net";
        private readonly string? _host = Environment.GetEnvironmentVariable("HiQ_AzureADTest_Address");
        private readonly string? _user = Environment.GetEnvironmentVariable("HiQ_AzureADTest_User");
        private readonly string? _pw = Environment.GetEnvironmentVariable("HiQ_AzureADTest_Password");
    */

    /*
        LDAP server to docker.
        docker run -d -it --rm -p 10389:10389 dwimberger/ldap-ad-it
    */
    private readonly string? _host = "127.0.0.1";
    private readonly int _port = 10389;
    private readonly string? _user = "uid=admin,ou=system";
    private readonly string? _pw = "secret";
    private readonly string _path = "ou=users,dc=wimpi,dc=net";
    private readonly string? _dn = "CN=Tes Tuser,ou=users,dc=wimpi,dc=net";
    private readonly string? _groupDn = "cn=admin,ou=roles,dc=wimpi,dc=net";

    Input? input;
    Connection? connection;

    [TestInitialize]
    public void StartUp()
    {
        try
        {
            CreateTestUsers();
        }
        catch (Exception)
        {
        }
    }

    [TestCleanup]
    public void CleanUp()
    {
        try
        {
            DeleteUser();
        }
        catch (Exception)
        {
        }
    }

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
        input = new()
        {
            UserDistinguishedName = _dn,
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

    public void CreateTestUsers()
    {
        LdapConnection conn = new();
        conn.Connect(_host, _port);
        conn.Bind(_user, _pw);

        var attributeSet = new LdapAttributeSet();
        attributeSet.Add(new LdapAttribute("objectclass", "user"));
        attributeSet.Add(new LdapAttribute("cn", "Tes Tuser"));
        attributeSet.Add(new LdapAttribute("givenname", "Tes"));
        attributeSet.Add(new LdapAttribute("sn", "Tuser"));

        var entry = $"CN=Tes Tuser,{_path}";
        LdapEntry newEntry = new(entry, attributeSet);
        conn.Add(newEntry);
        conn.Disconnect();
    }

    public void DeleteUser()
    {
        LdapConnection conn = new();
        conn.Connect(_host, _port);
        conn.Bind(_user, _pw);
        conn.Delete($"CN=Tes Tuser,{_path}");
        conn.Delete($"CN=Tes Tuser,{_path}");
        conn.Disconnect();
    }
}