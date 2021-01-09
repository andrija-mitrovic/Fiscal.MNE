using Fiscal.MNE.Services;

namespace Fiscal.MNE.Models
{
    public class TCRHeaderType
    {
        public RegisterTCRRequestHeaderType Header { get; set; }
        public TCRType TCR { get; set; }
    }
}
