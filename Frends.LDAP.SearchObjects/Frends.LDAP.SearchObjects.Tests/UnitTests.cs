using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.LDAP.SearchObjects.Definitions;
using Novell.Directory.Ldap;
namespace Frends.LDAP.SearchObjects.Tests;

[TestClass]
public class UnitTests
{

    /*
        Create a simple LDAP server to docker
        docker run -d -it --rm -p 10389:10389 dwimberger/ldap-ad-it
    */

    private readonly string? _host = "127.0.0.1";
    private readonly int _port = 10389;
    private readonly string? _user = "uid=admin,ou=system";
    private readonly string? _pw = "secret";
    private readonly string _path = "ou=users,dc=wimpi,dc=net";
    private readonly List<string> _cns = new() { "Tes Tuser", "Qwe Rty", "Foo Bar" };

    Input? input;
    Connection? connection;

    [TestInitialize]
    public void Setup()
    {
        CreateTestUsers();
    }

    [TestCleanup]
    public void CleanUp()
    {
        DeleteUsers();
    }

    [TestMethod]
    public void Search_Test()
    {
        input = new()
        {
            SearchBase = _path,
            Scope = Scopes.ScopeSub,
            Filter = "(ObjectClass=inetOrgPerson)",
            MsLimit = default,
            ServerTimeLimit = default,
            SearchDereference = SearchDereference.DerefNever,
            MaxResults = default,
            BatchSize = default,
            TypesOnly = default,
            Attributes = null,
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

        var result = LDAP.SearchObjects(input, connection);
        Assert.IsTrue(result.Success.Equals(true));
        //Assert.IsTrue(result.Success.Equals(false) && result.Error.Contains("No Such Object"));
    }

    public void CreateTestUsers()
    {
        LdapConnection conn = new();
        conn.Connect(_host, _port);
        conn.Bind(_user, _pw);

        foreach(var i in _cns)
        {
            LdapAttributeSet attributeSet = new();
            attributeSet.Add(new LdapAttribute("objectclass", "inetOrgPerson"));
            attributeSet.Add(new LdapAttribute("cn", i));
            attributeSet.Add(new LdapAttribute("givenname", i[..2]));
            attributeSet.Add(new LdapAttribute("sn", i[4..]));
            attributeSet.Add(new LdapAttribute("title", "engineer"));

            var entry = $"CN={i},{_path}";
            LdapEntry newEntry = new(entry, attributeSet);
            conn.Add(newEntry);
        }
        conn.Disconnect();
    }

    public void DeleteUsers()
    {
        LdapConnection conn = new();
        conn.Connect(_host, _port);
        conn.Bind(_user, _pw);

        foreach (var i in _cns)
            conn.Delete($"CN={i},{_path}");
        
        conn.Disconnect();
    }

}