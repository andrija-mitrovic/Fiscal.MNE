using Fiscal.MNE.Models;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Fiscal.MNE.Services
{
    internal class IICGenerator
    {
        private readonly X509Certificate2 _certificate;

        public IICGenerator(X509Certificate2 certificate)
        {
            _certificate = certificate;
        }

        public IICType Generate(IICConfig iicConfig)
        {
            IICValidator.ThrowExceptionIfNotValid(iicConfig);
            var inputParameter = GetInputParameterForIIC(iicConfig);

            // Load a private from a key store 
            RSA privateKey = _certificate.GetRSAPrivateKey();

            IICType iicType = new IICType();
            // Create IIC signature according to RSASSA-PKCS-v1_5 
            byte[] iicSignature = privateKey.SignData(Encoding.ASCII.GetBytes(inputParameter),
                HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            iicType.IICSignature = BitConverter.ToString(iicSignature).Replace("-", string.Empty);

            // Hash IIC signature with MD5 to create IIC 
            byte[] iic = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(iicSignature);
            iicType.IIC = BitConverter.ToString(iic).Replace("-", string.Empty);

            return iicType;
        }

        private string GetInputParameterForIIC(IICConfig iicConfig)
        {
            StringBuilder inputParameter = new StringBuilder();

            inputParameter.Append(iicConfig.IssuerTIN);
            inputParameter.Append("|" + iicConfig.DateTimeCreated);
            inputParameter.Append("|" + iicConfig.InvoiceNumber);
            inputParameter.Append("|" + iicConfig.BusinUnitCode);
            inputParameter.Append("|" + iicConfig.TCRCode);
            inputParameter.Append("|" + iicConfig.SoftCode);
            inputParameter.Append("|" + iicConfig.TotalPrice);

            return inputParameter.ToString();
        }
    }
}
