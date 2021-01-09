using Fiscal.MNE.Services;

namespace Fiscal.MNE.Interfaces
{
    internal interface IRegisterRequest
    {
        string Id { get; set; }
        SignatureType Signature { get; set; }
    }
}
