// Create a Factur-X PDF/A-3 invoice using the InvoiceXML API.
// Get API key: https://www.invoicexml.com/account/authentication
// Docs:        https://www.invoicexml.com/docs/api/create/facturx
//
// Dependency (Maven):
//   <dependency>
//     <groupId>com.squareup.okhttp3</groupId>
//     <artifactId>okhttp</artifactId>
//     <version>4.12.0</version>
//   </dependency>

import okhttp3.*;
import java.nio.file.Files;
import java.nio.file.Paths;

public class Create {
    public static void main(String[] args) throws Exception {
        String apiKey = "YOUR_API_KEY";

        RequestBody body = new MultipartBody.Builder()
            .setType(MultipartBody.FORM)
            .addFormDataPart("InvoiceNumber",          "INV-2025-001")
            .addFormDataPart("IssueDate",              "2025-07-01")
            .addFormDataPart("PaymentDueDate",         "2025-08-01")
            .addFormDataPart("Currency",               "EUR")
            .addFormDataPart("SellerName",             "Acme GmbH")
            .addFormDataPart("SellerLegalId",          "HRB98765")
            .addFormDataPart("SellerTaxId",            "DE123456789")
            .addFormDataPart("SellerStreet",           "Musterstr. 1")
            .addFormDataPart("SellerPostcode",         "10115")
            .addFormDataPart("SellerCity",             "Berlin")
            .addFormDataPart("SellerCountry",          "DE")
            .addFormDataPart("BuyerName",              "Example Corp")
            .addFormDataPart("BuyerStreet",            "12 Rue de Rivoli")
            .addFormDataPart("BuyerPostcode",          "75001")
            .addFormDataPart("BuyerCity",              "Paris")
            .addFormDataPart("BuyerCountry",           "FR")
            .addFormDataPart("Lines[0].Description",   "Consulting Services")
            .addFormDataPart("Lines[0].Quantity",      "10")
            .addFormDataPart("Lines[0].UnitPrice",     "150.00")
            .addFormDataPart("Lines[0].TaxPercentage", "19")
            .addFormDataPart("PaymentMeansCode",       "30")
            .addFormDataPart("IBAN",                   "DE89370400440532013000")
            .build();

        Request request = new Request.Builder()
            .url("https://api.invoicexml.com/v1/create/facturx")
            .header("Authorization", "Bearer " + apiKey)
            .post(body)
            .build();

        try (Response response = new OkHttpClient().newCall(request).execute()) {
            byte[] pdf = response.body().bytes();
            if (!response.isSuccessful()) {
                System.err.println("InvoiceXML API error " + response.code() + ": " + new String(pdf));
                System.exit(1);
            }
            Files.write(Paths.get("invoice-facturx.pdf"), pdf);
            System.out.println("Saved invoice-facturx.pdf (" + pdf.length + " bytes)");
        }
    }
}
