namespace Nsid.Tests;

public class ConformationTests
{
    [Fact]
    public void NsidDomainAuthority_ShouldBeLowercase_WhenUppercaseInputsAreUsed()
    {
        const string value = "cOm.eXaMpLe.hELLo";
        const string expected = "com.example";

        Nsid nsid = new(value);
        nsid.DomainAuthority.Should().Be(expected);
    }
    
    [Fact]
    public void NsidValue_ShouldHaveNameCasingBePreserved_WhenUppercaseInputsAreUsed()
    {
        const string value = "cOm.eXaMpLe.hELLo";
        const string expected = "com.example.hELLo";

        Nsid nsid = new(value);
        nsid.Value.Should().Be(expected);
    }
    
    [Fact]
    public void Nsid_ShouldNotHaveEmptySegments()
    {
        const string value = "com..example.hello";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.EmptySegment);
    }
    
    [Fact]
    public void Nsid_ShouldNotHaveEmptySegments_2()
    {
        const string value = "com.example..hello";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.EmptySegment);
    }
    
    [Fact]
    public void Nsid_ShouldNotStartWithSeparator()
    {
        const string value = ".com.example.hello";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.FirstSegmentStartsWithSeparator);
    }
    
    [Fact]
    public void Nsid_ShouldNotEndWithSeparator()
    {
        const string value = "com.example.hello.";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.NameEndsWithSeparator);
    }
}