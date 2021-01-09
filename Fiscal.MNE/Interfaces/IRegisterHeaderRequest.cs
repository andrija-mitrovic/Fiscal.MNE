using System;

namespace Fiscal.MNE.Interfaces
{
    internal interface IRegisterHeaderRequest
    {
        string UUID { get; set; }
        DateTime SendDateTime { get; set; }
    }
}
