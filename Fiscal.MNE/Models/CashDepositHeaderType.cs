using Fiscal.MNE.Services;

namespace Fiscal.MNE.Models
{
    public class CashDepositHeaderType
    {
        public RegisterCashDepositRequestHeaderType Header { get; set; }
        public CashDepositType CashDeposit { get; set; }
    }
}
