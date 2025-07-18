namespace Travel.Domain.Extensions;

public static class Guid
{
    public static System.Guid NewGuid() => System.Guid.NewGuid();
    public static string NewGuidAsString() => System.Guid.NewGuid().ToString();
    public static string NewGuidAsString(string format) => System.Guid.NewGuid().ToString(format);
}