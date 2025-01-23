namespace ECB.Infrastructure.Configuration;

public class ExternalApiOptions
{
    public const string SectionName = "ExternalApiSettings";
    public string EcbXmlUrl { get; set; } = null!;
}