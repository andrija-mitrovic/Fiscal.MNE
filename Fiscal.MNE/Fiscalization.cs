// The MIT License
// https://github.com/andrija-mitrovic/Fiscal.MNE
// Copyright (c) 2020-present Andrija Mitrovic
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Fiscal.MNE.Models;
using Fiscal.MNE.Services;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Fiscal.MNE
{
    public class Fiscalization
    {
        public const string DATE_FORMAT_SHORT = "yyyy-MM-dd";
        public const string DATE_FORMAT_MIDDLE = "yyyy-MM-ddTHH:mm:ss";
        public const string DATE_FORMAT_LONG = "yyyy-MM-ddTHH:mm:ss+01:00";

        private const string SERVICE_URL_PRODUCTION = "https://efi.tax.gov.me/fs-v1";
        private const string SERVICE_URL_TEST = "https://efitest.tax.gov.me/fs-v1";

        private readonly FiscalizationServiceProvider _fiscalService;
        private readonly X509Certificate2 _certificate;
        private readonly SignatureService _signatureService;

        public Fiscalization(X509Certificate2 certificate)
        {
            if (certificate == null) throw new ArgumentNullException("Certificate");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _fiscalService = new FiscalizationServiceProvider();
            _fiscalService.Url = SERVICE_URL_TEST;
            _certificate = certificate;
            _signatureService = new SignatureService();
        }

        public RegisterInvoiceResponse RegisterInvoice(InvoiceHeaderType invoiceHeaderType)
        {
            var request = GetRegisterInvoiceRequest(invoiceHeaderType);
            _signatureService.SignRequest(request, _certificate, invoiceHeaderType.IssuerTIN);

            return _fiscalService.registerInvoice(request);
        }

        public async Task<RegisterInvoiceResponse> RegisterInvoiceAsync(InvoiceHeaderType invoiceHeaderType)
        {
            var request = GetRegisterInvoiceRequest(invoiceHeaderType);
            _signatureService.SignRequest(request, _certificate, invoiceHeaderType.IssuerTIN);

            return await _fiscalService.RegisterInvoiceAsync(request);
        }

        public RegisterTCRResponse RegisterTCR(TCRHeaderType tcrHeaderType)
        {
            var request = GetRegisterTCRRequest(tcrHeaderType);
            _signatureService.SignRequest(request, _certificate);

            return _fiscalService.registerTCR(request);
        }

        public async Task<RegisterTCRResponse> RegisterTCRAsync(TCRHeaderType tcrHeaderType)
        {
            var request = GetRegisterTCRRequest(tcrHeaderType);
            _signatureService.SignRequest(request, _certificate);

            return await _fiscalService.RegisterTCRAsync(request);
        }

        public RegisterCashDepositResponse RegisterCashDeposit(CashDepositHeaderType cashDepositHeaderType)
        {
            var request = GetRegisterCashDepositRequest(cashDepositHeaderType);
            _signatureService.SignRequest(request, _certificate);

            return _fiscalService.registerCashDeposit(request);
        }

        public async Task<RegisterCashDepositResponse> RegisterCashDepositAsync(CashDepositHeaderType cashDepositHeaderType)
        {
            var request = GetRegisterCashDepositRequest(cashDepositHeaderType);
            _signatureService.SignRequest(request, _certificate);

            return await _fiscalService.RegisterCashDepositAsync(request);
        }

        private RegisterInvoiceRequest GetRegisterInvoiceRequest(InvoiceHeaderType invoiceHeaderType)
        {
            if (invoiceHeaderType == null) throw new ArgumentNullException("InvoiceHeaderType");

            return new RegisterInvoiceRequest
            {
                Header = invoiceHeaderType.Header,
                Invoice = invoiceHeaderType.Invoice
            };
        }

        private RegisterTCRRequest GetRegisterTCRRequest(TCRHeaderType tcrHeaderType)
        {
            if (tcrHeaderType == null) throw new ArgumentNullException("TCRHeaderType");

            return new RegisterTCRRequest
            {
                Header = tcrHeaderType.Header,
                TCR = tcrHeaderType.TCR
            };
        }

        private RegisterCashDepositRequest GetRegisterCashDepositRequest(CashDepositHeaderType cashDepositHeaderType)
        {
            if (cashDepositHeaderType == null) throw new ArgumentNullException("CashDepositHeaderType");

            return new RegisterCashDepositRequest
            {
                Header = cashDepositHeaderType.Header,
                CashDeposit = cashDepositHeaderType.CashDeposit
            };
        }
    }
}
