// Create a Factur-X PDF/A-3 invoice with embedded EN 16931 XML using the
// InvoiceXML REST API.
//
// Get your API key at: https://www.invoicexml.com/account/authentication
// Full API reference:  https://www.invoicexml.com/docs/api/create/facturx
//
// Required NuGet package:
//   dotnet add package Flurl.Http

using Flurl.Http;

public static class CreateFacturX
{
    private const string Endpoint = "https://api.invoicexml.com/v1/create/facturx";

    public static async Task<byte[]> RunAsync(string outputPath = "invoice-facturx.pdf")
    {
        // Get an InvoiceXML API key at: https://www.invoicexml.com/account/authentication
        var apiKey = "INVOICEXML_API_KEY";

        try
        {
            // POST multipart/form-data with the invoice fields.
            // Full field list and validation rules:
            // https://www.invoicexml.com/docs/api/create/facturx
            var pdfBytes = await Endpoint
                .WithOAuthBearerToken(apiKey)
                .PostMultipartAsync(mp => mp
                    // Header
                    .AddString("InvoiceNumber",  "INV-2025-001")
                    .AddString("IssueDate",      "2025-07-01")
                    .AddString("PaymentDueDate", "2025-08-01")
                    .AddString("Currency",       "EUR")
                    // Seller
                    .AddString("SellerName",     "Acme GmbH")
                    .AddString("SellerLegalId",  "HRB98765")
                    .AddString("SellerTaxId",    "DE123456789")
                    .AddString("SellerStreet",   "Musterstr. 1")
                    .AddString("SellerPostcode", "10115")
                    .AddString("SellerCity",     "Berlin")
                    .AddString("SellerCountry",  "DE")
                    // Buyer
                    .AddString("BuyerName",      "Example Corp")
                    .AddString("BuyerStreet",    "12 Rue de Rivoli")
                    .AddString("BuyerPostcode",  "75001")
                    .AddString("BuyerCity",      "Paris")
                    .AddString("BuyerCountry",   "FR")
                    // Line items (repeat with Lines[1], Lines[2], etc. for more)
                    .AddString("Lines[0].Description",   "Consulting Services")
                    .AddString("Lines[0].Quantity",      "10")
                    .AddString("Lines[0].UnitPrice",     "150.00")
                    .AddString("Lines[0].TaxPercentage", "19")
                    // Payment
                    .AddString("PaymentMeansCode", "30")
                    .AddString("IBAN",             "DE89370400440532013000")
                )
                .ReceiveBytes();

            await File.WriteAllBytesAsync(outputPath, pdfBytes);
            Console.WriteLine($"Factur-X invoice saved: {outputPath} ({pdfBytes.Length:N0} bytes)");
            return pdfBytes;
        }
        catch (FlurlHttpException ex)
        {
            var body = await ex.GetResponseStringAsync();
            Console.Error.WriteLine($"InvoiceXML API error {(int?)ex.StatusCode}: {body}");
            throw;
        }
    }
}
