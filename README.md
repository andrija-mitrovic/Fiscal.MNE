# Fiscal.MNE
.NET clinet library for access to the DPR (Department of Public Revenues) Web Service

Folder FZWS.ConsoleUI contains examples of how to use Fiscal.MNE library. View file *FZWS.ConsoleUI/Program.cs*

Microsoft's [wsdl.exe](https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-2.0/7h3ystb6(v=vs.80)?redirectedfrom=MSDN) tool was used to generate a proxy class with the structure according to the wsdl scheme published on the Department of Public Revenues's website [Tehnical specification](https://poreskauprava.gov.me/vijesti/237406/ELEKTRONSKA-FISKALIZACIJA-Nove-verzije-dokumentacije.html) and which is included in the project source code. All SOAP calls to the Web Service work through the generated SOAP client FiscalizationService class. The complete implementation is in the Fiscalization class which contains sync and async methods.

# Usage
Fiscalization class contains two type of methods (sync and async) for sending data to the service:

```csharp
    public class Fiscalization
    {
        public RegisterInvoiceResponse RegisterInvoice(InvoiceHeaderType invoiceHeaderType) {...}
        public async Task<RegisterInvoiceResponse> RegisterInvoiceAsync(InvoiceHeaderType invoiceHeaderType) {...}
        public RegisterTCRResponse RegisterTCR(TCRHeaderType tcrHeaderType) {...}
        async Task<RegisterTCRResponse> RegisterTCRAsync(TCRHeaderType tcrHeaderType) {...}
        public RegisterCashDepositResponse RegisterCashDeposit(CashDepositHeaderType cashDepositHeaderType) {...}
        public async Task<RegisterCashDepositResponse> RegisterCashDepositAsync(CashDepositHeaderType cashDepositHeaderType) {...}
    }
```
Service call example:

    var invoiceHeaderType = ...;

    using (X509Certificate2 certificate = new X509Certificate2(KEYSTORE_LOCATION, KEYSTORE_PASS))
    {
        Fiscalization fiscalization = new Fiscalization(certificate);
        var response = fiscalization.RegisterInvoice(invoiceHeaderType);
    }

# Open source license
    The MIT License

    Copyright (c) 2020 Andrija Mitrovic

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
