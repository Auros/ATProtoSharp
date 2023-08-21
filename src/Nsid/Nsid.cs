using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using Nsid.Exceptions;

[assembly: InternalsVisibleTo("Nsid.Tests")]
namespace Nsid;

/// <summary>
/// https://atproto.com/specs/nsid
/// </summary>
public sealed class Nsid
{
    /// <summary>
    /// The name of the NSID.
    /// <example>
    /// In the example NSID com.example.hello, "hello" is the name.
    /// </example>
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// The value of the NSID.
    /// <example>
    /// In the example NSID com.example.hello, "com.example.hello", the entire string, is the value.
    /// It is also the value of .ToString()
    /// </example>
    /// </summary>
    public string Value { get; }
    
    /// <summary>
    /// The domain authority of the NSID.
    /// <example>
    /// In the example NSID com.example.hello, "com.example", is the domain authority.
    /// </example>
    /// </summary>
    public string DomainAuthority { get; }
    
    /// <summary>
    /// The segments of the NSID.
    /// <example>
    /// In the example NSID com.example.hello, "com", "example", and "hello" are the segments.
    /// </example>
    /// </summary>
    public IReadOnlyList<string> Segments { get; }

    public Nsid(string nsidString)
    {
        var createResult = TryCreateInternal(nsidString, out var name, out var value, out var domainAuthority, out var segments);
        if (createResult is not CreateResult.Success)
            throw new InvalidNsidException(createResult);

        Name = name;
        Value = value;
        Segments = segments;
        DomainAuthority = domainAuthority;
    }

    private Nsid(string name, string value, string domainAuthority, IReadOnlyList<string> segments)
    {
        Name = name;
        Value = value;
        Segments = segments;
        DomainAuthority = domainAuthority;
    }

    public static bool TryCreate(string nsidString, [NotNullWhen(true)] out Nsid? result)
    {
        var createResult = TryCreateInternal(nsidString, out var name, out var value, out var domainAuthority, out var segments);
        result = new Nsid(name, value, domainAuthority, segments);
        return createResult is CreateResult.Success;
    }

    private static CreateResult TryCreateInternal(string nsidString, out string name, out string value, out string domainAuthority, out List<string> segments)
    {
        name = string.Empty;
        value = string.Empty;
        domainAuthority = string.Empty;
        segments = null!;
        if (string.IsNullOrWhiteSpace(nsidString))
            return CreateResult.Empty;

        if (nsidString.Length > 317)
            return CreateResult.OverallNSIDTooLong;

        var firstCharacter = nsidString[0];
        switch (firstCharacter)
        {
            case '-':
                return CreateResult.DomainAuthoritySegmentStartsWithHyphen;
            case '.':
                return CreateResult.FirstSegmentStartsWithSeparator;
        }

        if (char.IsAsciiDigit(firstCharacter))
            return CreateResult.FirstSegmentStartsWithDigit;

        if (nsidString[^1] is '.')
            return CreateResult.NameEndsWithSeparator;
        
        var raw = nsidString.AsSpan();
        segments = new List<string>();

        int reader = 0;
        for (int i = 0; i < raw.Length; i++)
        {
            var c = raw[i];
            if (c is not '.' && c is not '-' && !char.IsAsciiLetterOrDigit(c))
                return CreateResult.ContainsForbiddenCharacters;

            if (c is not '.')
                continue;
            
            // We can safely check the previous element here since
            // above we guard if the first character is a period.
            var previousCharacter = raw[i - 1];
            switch (previousCharacter)
            {
                case '-':
                    return CreateResult.DomainAuthoritySegmentEndsWithHyphen;
                case '.':
                    return CreateResult.EmptySegment;
                default:
                    var length = i - reader;
                    
#pragma warning disable CA2014
                    // ReSharper disable once StackAllocInsideLoop
                    // At most there will be 317 loops. This will not overflow.
                    Span<char> temp = stackalloc char[length];
#pragma warning restore CA2014
                    
                    // Lowercasing the string as the spec requires domain authority segments to be normalized.
                    raw.Slice(reader, i - reader).ToLowerInvariant(temp);
                    segments.Add(temp.ToString());
                    reader = i + 1;
                    break;
            }
        }

        // The reader should stop on the
        // character after the last separator.
        var nameLength = raw.Length - reader;
        if (nameLength > 63)
            return CreateResult.NameTooLong;

        name = raw[reader..].ToString();
        // ReSharper disable once LoopCanBeConvertedToQuery (Avoiding unnecessary allocation on potentially hot path)
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (!char.IsAsciiLetter(c))
                return CreateResult.ContainsForbiddenCharacters;
        }

        segments.Add(raw[reader..].ToString());

        if (3 > segments.Count)
            return CreateResult.NotEnoughSegments;

        StringBuilder sb = new(reader);
        for (int i = 0; i < segments.Count - 1; i++)
        {
            var segment = segments[i];
            if (segment[0] is '-')
                return CreateResult.DomainAuthoritySegmentStartsWithHyphen;
            
            if (segment.Length > 63)
                return CreateResult.DomainAuthoritySegmentTooLong;
            
            sb.Append(segment).Append('.');
        }
        sb.Append(name);

        value = sb.ToString();
        domainAuthority = value[..(sb.Length - (name.Length + 1))];
        return domainAuthority.Length > 253 ? CreateResult.DomainAuthorityTooLong : CreateResult.Success;
    }

    public override string ToString()
    {
        return Value;
    }
    
    internal enum CreateResult
    {
        Success,
        Empty,
        EmptySegment,
        NotEnoughSegments,
        OverallNSIDTooLong,
        ContainsForbiddenCharacters,
        NameTooLong,
        DomainAuthorityTooLong,
        DomainAuthoritySegmentTooLong,
        DomainAuthoritySegmentStartsWithHyphen,
        DomainAuthoritySegmentEndsWithHyphen,
        FirstSegmentStartsWithSeparator,
        FirstSegmentStartsWithDigit,
        NameEndsWithSeparator
    }
}