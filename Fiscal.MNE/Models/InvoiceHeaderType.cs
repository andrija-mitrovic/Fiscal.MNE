using Fiscal.MNE.Services;

namespace Fiscal.MNE.Models
{
    public class InvoiceHeaderType
    {
        public string IssuerTIN { get; set; }
        public RegisterInvoiceRequestHeaderType Header { get; set; }
        public InvoiceType Invoice { get; set; }
    }
}
