using Fiscal.MNE.Services;

namespace Fiscal.MNE.Interfaces
{
    internal interface IAdditionalRegisterHeaderRequest : IRegisterHeaderRequest
    {
        SubseqDelivTypeSType SubseqDelivType { get; set; }
        bool SubseqDelivTypeSpecified { get; set; }
    }
}
