namespace Nsid.Tests;

public class ValidityTests
{
    [Fact]
    public void ValidNsid_1()
    {
        Nsid nsid = new("com.example.hello");
        
        nsid.Name.Should().Be("hello");
        nsid.Value.Should().Be("com.example.hello");
        nsid.DomainAuthority.Should().Be("com.example");
        nsid.Segments.Should().ContainInOrder("com", "example", "hello");
    }
    
    [Fact]
    public void ValidNsid_2()
    {
        Nsid nsid = new("com.example.fooBar");
        
        nsid.Name.Should().Be("fooBar");
        nsid.Value.Should().Be("com.example.fooBar");
        nsid.DomainAuthority.Should().Be("com.example");
        nsid.Segments.Should().ContainInOrder("com", "example", "fooBar");
    }
    
    [Fact]
    public void ValidNsid_3()
    {
        Nsid nsid = new("net.users.bob.ping");
        
        nsid.Name.Should().Be("ping");
        nsid.Value.Should().Be("net.users.bob.ping");
        nsid.DomainAuthority.Should().Be("net.users.bob");
        nsid.Segments.Should().ContainInOrder("net", "users", "bob", "ping");
    }
    
    [Fact]
    public void ValidNsid_4()
    {
        Nsid nsid = new("a-0.b-1.c");
        
        nsid.Name.Should().Be("c");
        nsid.Value.Should().Be("a-0.b-1.c");
        nsid.DomainAuthority.Should().Be("a-0.b-1");
        nsid.Segments.Should().ContainInOrder("a-0", "b-1", "c");
    }
    
    [Fact]
    public void ValidNsid_5()
    {
        Nsid nsid = new("a.b.c");
        
        nsid.Name.Should().Be("c");
        nsid.Value.Should().Be("a.b.c");
        nsid.DomainAuthority.Should().Be("a.b");
        nsid.Segments.Should().ContainInOrder("a", "b", "c");
    }
    
    [Fact]
    public void ValidNsid_6()
    {
        Nsid nsid = new("cn.8.lex.stuff");
        
        nsid.Name.Should().Be("stuff");
        nsid.Value.Should().Be("cn.8.lex.stuff");
        nsid.DomainAuthority.Should().Be("cn.8.lex");
        nsid.Segments.Should().ContainInOrder("cn", "8", "lex", "stuff");
    }
}