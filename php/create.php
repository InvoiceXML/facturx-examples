<?php
// Create a Factur-X PDF/A-3 invoice using the InvoiceXML API.
// Get API key: https://www.invoicexml.com/account/authentication
// Docs:        https://www.invoicexml.com/docs/api/create/facturx

$apiKey = 'YOUR_API_KEY';

$fields = [
    'InvoiceNumber'          => 'INV-2025-001',
    'IssueDate'              => '2025-07-01',
    'PaymentDueDate'         => '2025-08-01',
    'Currency'               => 'EUR',
    'SellerName'             => 'Acme GmbH',
    'SellerLegalId'          => 'HRB98765',
    'SellerTaxId'            => 'DE123456789',
    'SellerStreet'           => 'Musterstr. 1',
    'SellerPostcode'         => '10115',
    'SellerCity'             => 'Berlin',
    'SellerCountry'          => 'DE',
    'BuyerName'              => 'Example Corp',
    'BuyerStreet'            => '12 Rue de Rivoli',
    'BuyerPostcode'          => '75001',
    'BuyerCity'              => 'Paris',
    'BuyerCountry'           => 'FR',
    'Lines[0].Description'   => 'Consulting Services',
    'Lines[0].Quantity'      => '10',
    'Lines[0].UnitPrice'     => '150.00',
    'Lines[0].TaxPercentage' => '19',
    'PaymentMeansCode'       => '30',
    'IBAN'                   => 'DE89370400440532013000',
];

$ch = curl_init('https://api.invoicexml.com/v1/create/facturx');
curl_setopt_array($ch, [
    CURLOPT_RETURNTRANSFER => true,
    CURLOPT_POST           => true,
    CURLOPT_POSTFIELDS     => $fields,
    CURLOPT_HTTPHEADER     => ['Authorization: Bearer ' . $apiKey],
]);

$pdf    = curl_exec($ch);
$status = curl_getinfo($ch, CURLINFO_HTTP_CODE);
curl_close($ch);

if ($status !== 200) {
    fwrite(STDERR, "InvoiceXML API error $status: $pdf\n");
    exit(1);
}

file_put_contents('invoice-facturx.pdf', $pdf);
echo "Saved invoice-facturx.pdf (" . strlen($pdf) . " bytes)\n";
