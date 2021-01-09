using Fiscal.MNE.Helpers;
using Fiscal.MNE.Interfaces;
using Fiscal.MNE.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Fiscal.MNE.Services
{
    internal class SignatureService
    {
        public void SignRequest(IRegisterRequest request, X509Certificate2 certificate, string issuerTIN = null)
        {
            if (request is RegisterInvoiceRequest)
            {
                var invoiceRequest = request as RegisterInvoiceRequest;
                var invoiceSignature = GetIICandIICSignature(invoiceRequest, certificate, issuerTIN);
                if (invoiceSignature != null)
                {
                    invoiceRequest.Invoice.IICSignature = invoiceSignature.IICSignature;
                    invoiceRequest.Invoice.IIC = invoiceSignature.IIC;
                }
            }

            XmlDocument xmlDocument = LoadXmlDocument(request);
            SignedXml xml = SignXmlDocument(xmlDocument, certificate);

            var transforms = new Transform[]
            {
                new XmlDsigEnvelopedSignatureTransform(false),
                new XmlDsigExcC14NTransform(false)
            };

            var reference = GetReference(transforms, "#" + request.Id);
            xml.AddReference(reference);
            xml.ComputeSignature();

            request.Signature = GetSignatureType(xml.Signature, transforms, certificate);
        }

        private Reference GetReference(Transform[] transforms, string uri)
        {
            Reference reference = new Reference(uri);
            foreach (var x in transforms)
                reference.AddTransform(x);
            reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";

            return reference;
        }

        private SignedXml SignXmlDocument(XmlDocument xmlDocument, X509Certificate2 certificate)
        {
            RSA privateKey = certificate.GetRSAPrivateKey();

            SignedXml xml = new SignedXml(xmlDocument);
            xml.SigningKey = privateKey;
            xml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            xml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
            xml.KeyInfo = GetKeyInfo(certificate);

            return xml;
        }

        private XmlDocument LoadXmlDocument(IRegisterRequest request)
        {
            string serialize = SerializationHelper.SerializeDataToXmlString(request);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(serialize);

            return xmlDocument;
        }

        private KeyInfo GetKeyInfo(X509Certificate2 certificate)
        {
            KeyInfo keyInfo = new KeyInfo();
            KeyInfoX509Data keyInfoData = new KeyInfoX509Data();
            keyInfoData.AddCertificate(certificate);
            keyInfo.AddClause(keyInfoData);

            return keyInfo;
        }

        private IICType GetIICandIICSignature(RegisterInvoiceRequest invoiceRequest, X509Certificate2 certificate, string issuerTIN)
        {
            IICType iicType = null;
            if (invoiceRequest.Signature != null) throw new Exception("Invoice already signed");

            if (invoiceRequest != null && invoiceRequest.Invoice.IIC == null &&
                invoiceRequest.Invoice.IICSignature == null)
            {
                InvoiceType invoice = invoiceRequest.Invoice;
                IICGenerator iicTypeGenerator = new IICGenerator(certificate);

                IICConfig iicConfig = new IICConfig()
                {
                    IssuerTIN = issuerTIN,
                    DateTimeCreated = invoice.IssueDateTime.ToString(Fiscalization.DATE_FORMAT_LONG),
                    InvoiceNumber = invoice.InvNum,
                    BusinUnitCode = invoice.BusinUnitCode,
                    TCRCode = invoice.TCRCode,
                    SoftCode = invoice.SoftCode,
                    TotalPrice = invoice.TotPrice.ToString()
                };

                iicType = iicTypeGenerator.Generate(iicConfig);
            }

            return iicType;
        }

        private SignatureType GetSignatureType(Signature signature, Transform[] transforms, X509Certificate2 certificate)
        {
            return new SignatureType
            {
                SignedInfo = GetSignedInfoType(signature, transforms),
                SignatureValue = new SignatureValueType { Value = signature.SignatureValue },
                KeyInfo = GetKeyInfoType(certificate)
            };
        }

        private SignedInfoType GetSignedInfoType(Signature signature, Transform[] transforms)
        {
            return new SignedInfoType
            {
                CanonicalizationMethod = new CanonicalizationMethodType { Algorithm = signature.SignedInfo.CanonicalizationMethod },
                SignatureMethod = new SignatureMethodType { Algorithm = signature.SignedInfo.SignatureMethod },
                Reference =
                        (from x in signature.SignedInfo.References.OfType<Reference>()
                         select new ReferenceType
                         {
                             URI = x.Uri,
                             Transforms =
                                 (from t in transforms
                                  select new TransformType { Algorithm = t.Algorithm }).ToArray(),
                             DigestMethod = new DigestMethodType { Algorithm = x.DigestMethod },
                             DigestValue = x.DigestValue
                         }).ToArray()
            };
        }

        private KeyInfoType GetKeyInfoType(X509Certificate2 certificate)
        {
            return new KeyInfoType
            {
                ItemsElementName = new[] { ItemsChoiceType2.X509Data },
                Items = new[]
                    {
                        new X509DataType
                        {
                            ItemsElementName = new[]
                            {
                                ItemsChoiceType.X509Certificate
                            },
                            Items = new object[]
                            {
                                certificate.RawData
                            }
                        }
                    }
            };
        }
    }
}
