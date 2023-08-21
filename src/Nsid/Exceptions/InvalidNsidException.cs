namespace Nsid.Exceptions;

public sealed class InvalidNsidException : Exception
{
    internal Nsid.CreateResult Result { get; }
    
    internal InvalidNsidException(Nsid.CreateResult result) : base(GetMessageFromResult(result))
    {
        Result = result;
    }

    private static string GetMessageFromResult(Nsid.CreateResult result)
    {
        return result switch
        {
            Nsid.CreateResult.Empty => "Cannot be empty.",
            Nsid.CreateResult.EmptySegment => "Cannot contain empty segments.",
            Nsid.CreateResult.NotEnoughSegments => "Requires a minimum of 3 segments.",
            Nsid.CreateResult.OverallNSIDTooLong => "NSID is too long. Maximum length is 317.",
            Nsid.CreateResult.ContainsForbiddenCharacters => "Contains forbidden characters.",
            Nsid.CreateResult.NameTooLong => "NSID Name is too long. Maximum length is 63",
            Nsid.CreateResult.DomainAuthorityTooLong => "Domain Authority is too long. Maximum length is 253",
            Nsid.CreateResult.DomainAuthoritySegmentTooLong => "Domain Authority segment is too long. Maximum length is 63.",
            Nsid.CreateResult.DomainAuthoritySegmentStartsWithHyphen => "Domain Authority segments cannot start with a hyphen.",
            Nsid.CreateResult.DomainAuthoritySegmentEndsWithHyphen => "Domain Authority segments cannot end with a hyphen.",
            Nsid.CreateResult.FirstSegmentStartsWithSeparator => "The first segment cannot start with a separator.",
            Nsid.CreateResult.FirstSegmentStartsWithDigit => "The first segment cannot start with a digit.",
            Nsid.CreateResult.NameEndsWithSeparator => "Name cannot end with a separator",
            Nsid.CreateResult.Success => "Invalid Exception",
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
    }
}