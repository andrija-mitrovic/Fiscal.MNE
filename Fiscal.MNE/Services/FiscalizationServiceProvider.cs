using Fiscal.MNE.Interfaces;
using System.Threading.Tasks;

namespace Fiscal.MNE.Services
{
    internal class FiscalizationServiceProvider : FiscalizationService
    {
        public Task<RegisterInvoiceResponse> RegisterInvoiceAsync(RegisterInvoiceRequest request)
        {
            return Task.Factory.FromAsync(BeginregisterInvoice, EndregisterInvoice, request, null);
        }

        public Task<RegisterTCRResponse> RegisterTCRAsync(RegisterTCRRequest request)
        {
            return Task.Factory.FromAsync(BeginregisterTCR, EndregisterTCR, request, null);
        }

        public Task<RegisterCashDepositResponse> RegisterCashDepositAsync(RegisterCashDepositRequest request)
        {
            return Task.Factory.FromAsync(BeginregisterCashDeposit, EndregisterCashDeposit, request, null);
        }
    }

    public partial class RegisterInvoiceRequest : IRegisterRequest { }

    public partial class RegisterTCRRequest : IRegisterRequest { }

    public partial class RegisterCashDepositRequest : IRegisterRequest { }

    public partial class RegisterTCRRequestHeaderType : IRegisterHeaderRequest { }

    public partial class RegisterCashDepositRequestHeaderType : IAdditionalRegisterHeaderRequest { }

    public partial class RegisterInvoiceRequestHeaderType : IAdditionalRegisterHeaderRequest { }
}
