# Create a Factur-X PDF/A-3 invoice using the InvoiceXML API.
# Get API key: https://www.invoicexml.com/account/authentication
# Docs:        https://www.invoicexml.com/docs/api/create/facturx
#
# Dependency:  pip install requests

import requests
import sys

api_key = "YOUR_API_KEY"

fields = {
    "InvoiceNumber":          "INV-2025-001",
    "IssueDate":              "2025-07-01",
    "PaymentDueDate":         "2025-08-01",
    "Currency":               "EUR",
    "SellerName":             "Acme GmbH",
    "SellerLegalId":          "HRB98765",
    "SellerTaxId":            "DE123456789",
    "SellerStreet":           "Musterstr. 1",
    "SellerPostcode":         "10115",
    "SellerCity":             "Berlin",
    "SellerCountry":          "DE",
    "BuyerName":              "Example Corp",
    "BuyerStreet":            "12 Rue de Rivoli",
    "BuyerPostcode":          "75001",
    "BuyerCity":              "Paris",
    "BuyerCountry":           "FR",
    "Lines[0].Description":   "Consulting Services",
    "Lines[0].Quantity":      "10",
    "Lines[0].UnitPrice":     "150.00",
    "Lines[0].TaxPercentage": "19",
    "PaymentMeansCode":       "30",
    "IBAN":                   "DE89370400440532013000",
}

# Send as multipart/form-data to match the documented curl -F behavior.
response = requests.post(
    "https://api.invoicexml.com/v1/create/facturx",
    headers={"Authorization": f"Bearer {api_key}"},
    files={k: (None, v) for k, v in fields.items()},
)

if not response.ok:
    sys.stderr.write(f"InvoiceXML API error {response.status_code}: {response.text}\n")
    sys.exit(1)

with open("invoice-facturx.pdf", "wb") as f:
    f.write(response.content)

print(f"Saved invoice-facturx.pdf ({len(response.content)} bytes)")
