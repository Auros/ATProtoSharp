

namespace Nsid.Tests;

public class CharacterTests
{
    [Fact]
    public void Nsid_ShouldThrow_WithNonValidASCIICodeInDomainAuthority()
    {
        const string value = "com.ex#mple.hello";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.ContainsForbiddenCharacters);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WithNonValidASCIICodeInName()
    {
        const string value = "com.example.h#llo";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.ContainsForbiddenCharacters);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WithUnicodeEmojiInDomainAuthority()
    {
        const string value = "com.exa💜mple.hello";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.ContainsForbiddenCharacters);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WithUnicodeEmojiInName()
    {
        const string value = "com.example.hel💜lo";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.ContainsForbiddenCharacters);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WithDomainAuthorityStartingWithHyphen()
    {
        const string value = "-com.example.hello";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.DomainAuthoritySegmentStartsWithHyphen);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WithDomainAuthoritySegmentStartingWithHyphen()
    {
        const string value = "com.-example.hello";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.DomainAuthoritySegmentStartsWithHyphen);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WithDomainAuthoritySegmentEndingWithHyphen()
    {
        const string value = "com.example-.hello";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.DomainAuthoritySegmentEndsWithHyphen);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WithDomainAuthorityStartingWithDigit()
    {
        const string value = "5om.example.hello";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.FirstSegmentStartsWithDigit);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WithDigitInName()
    {
        const string value = "com.example.h3llo";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.ContainsForbiddenCharacters);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WithHyphenInName()
    {
        const string value = "com.example.h-llo";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.ContainsForbiddenCharacters);
    }
}