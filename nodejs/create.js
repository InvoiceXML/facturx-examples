// Create a Factur-X PDF/A-3 invoice using the InvoiceXML API.
// Get API key: https://www.invoicexml.com/account/authentication
// Docs:        https://www.invoicexml.com/docs/api/create/facturx
//
// Requires Node.js 18+. No dependencies.

const fs = require('fs');

const apiKey = 'YOUR_API_KEY';

const fields = {
    'InvoiceNumber':          'INV-2025-001',
    'IssueDate':              '2025-07-01',
    'PaymentDueDate':         '2025-08-01',
    'Currency':               'EUR',
    'SellerName':             'Acme GmbH',
    'SellerLegalId':          'HRB98765',
    'SellerTaxId':            'DE123456789',
    'SellerStreet':           'Musterstr. 1',
    'SellerPostcode':         '10115',
    'SellerCity':             'Berlin',
    'SellerCountry':          'DE',
    'BuyerName':              'Example Corp',
    'BuyerStreet':            '12 Rue de Rivoli',
    'BuyerPostcode':          '75001',
    'BuyerCity':              'Paris',
    'BuyerCountry':           'FR',
    'Lines[0].Description':   'Consulting Services',
    'Lines[0].Quantity':      '10',
    'Lines[0].UnitPrice':     '150.00',
    'Lines[0].TaxPercentage': '19',
    'PaymentMeansCode':       '30',
    'IBAN':                   'DE89370400440532013000',
};

(async () => {
    const form = new FormData();
    for (const [key, value] of Object.entries(fields)) {
        form.append(key, value);
    }

    const response = await fetch('https://api.invoicexml.com/v1/create/facturx', {
        method: 'POST',
        headers: { Authorization: `Bearer ${apiKey}` },
        body: form,
    });

    if (!response.ok) {
        console.error(`InvoiceXML API error ${response.status}: ${await response.text()}`);
        process.exit(1);
    }

    const buffer = Buffer.from(await response.arrayBuffer());
    fs.writeFileSync('invoice-facturx.pdf', buffer);
    console.log(`Saved invoice-facturx.pdf (${buffer.length} bytes)`);
})();
