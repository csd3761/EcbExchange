using System.Xml.Linq;
using ECB.Domain.Models;

namespace ECB.Infrastructure.Parsers;

public static class EcbRatesParser
{
    public static EcbCurrencyRatesResponse ParseEcbRatesFromXml(string xmlContent)
    {
        if (string.IsNullOrWhiteSpace(xmlContent))
            throw new ArgumentException("XML content cannot be null or empty.");

        try
        {
            XDocument document = XDocument.Parse(xmlContent);

            XNamespace gesmes = "http://www.gesmes.org/xml/2002-08-01";
            XNamespace ecb = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

            var timeCube = document.Descendants(ecb + "Cube")
                .FirstOrDefault(c => c.Attribute("time") != null);

            if (timeCube == null)
            {
                throw new Exception("No Cube element with a 'time' attribute found.");
            }

            // Parse the date
            var time = timeCube?.Attribute("time")?.Value 
                       ?? throw new InvalidOperationException("Missing date attribute");

            // Parse the rates
            var rates = timeCube.Elements(ecb + "Cube")
                .Select(c => new CurrencyRate
                {
                    Currency = c.Attribute("currency")?.Value ?? throw new InvalidOperationException("Missing currency attribute"),
                    Rate = decimal.Parse(c.Attribute("rate")?.Value ?? throw new InvalidOperationException("Missing rate attribute"))
                })
                .ToList();

            // Create the response
            return new EcbCurrencyRatesResponse
            {
                Time = time,
                Rates = rates
            };
        }
        catch (Exception ex)
        {
            // Handle or log exceptions
            throw new Exception("Failed to parse ECB rates XML.", ex);
        }
    }
}
