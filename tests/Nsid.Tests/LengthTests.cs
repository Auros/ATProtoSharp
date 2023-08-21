namespace Nsid.Tests;

public class LengthTests
{
    [Fact]
    public void Nsid_ShouldThrow_WhenLongerThan317Characters()
    {
        var value = Enumerable.Range(0, 6).Select(_ => RandomChars(63)).Aggregate((a, b) => $"{a}.{b}");
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.OverallNSIDTooLong);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WhenDomainAuthoritySegmentIsLongerThan63Characters()
    {
        var value = $"com.{RandomChars(64)}.hello";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.DomainAuthoritySegmentTooLong);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WhenNameIsLongerThan63Characters()
    {
        var value = $"com.example.{RandomChars(64)}";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.NameTooLong);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WhenDomainAuthorityIsLongerThan253Characters()
    {
        var value = $"{RandomChars(63)}.{RandomChars(63)}.{RandomChars(63)}.{RandomChars(62)}.hello";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.DomainAuthorityTooLong);
    }
    
    [Fact]
    public void Nsid_ShouldThrow_WhenSegmentCountIsLessThan3()
    {
        const string value = $"com.example";
        Invoking(() => new Nsid(value)).Should().Throw<InvalidNsidException>()
            .And.Result.Should().Be(CreateResult.NotEnoughSegments);
    }

    private static string RandomChars(int length) => string.Concat(Enumerable.Range(0, length).Select(_ => (char)Random.Shared.Next(97, 123)));
}