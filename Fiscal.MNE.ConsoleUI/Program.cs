using Fiscal.MNE.Models;
using Fiscal.MNE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

namespace Fiscal.MNE.ConsoleUI
{
    class Program
    {
        private const String KEYSTORE_LOCATION = @"*******.pfx";
        private const String KEYSTORE_PASS = "*******";

        static void Main(string[] args)
        {
            Start();
        }

        private static void Start()
        {
            Console.WriteLine("============================");
            Console.WriteLine("1. ENU Registration CASH");
            Console.WriteLine("2. Register Cash Deposit CASH");
            Console.WriteLine("3. Invoice Register CASH");
            Console.WriteLine("4. ENU Registration NONCASH");
            Console.WriteLine("5. Invoice Register NONCASH");

            var res = Console.ReadLine();
            var broj = 0;
            if (Int32.TryParse(res, out broj))
            {
                try
                {
                    switch (broj)
                    {
                        case 1:
                            TestENURegistrationCash();
                            break;
                        case 2:
                            TestRegisterCashDepositCash();
                            break;
                        case 3:
                            TestInvoiceRegistrationCash();
                            break;
                        case 4:
                            TestENURegistrationNoncash();
                            break;
                        case 5:
                            TestInvoiceRegistrationNoncash();
                            break;
                    }
                }
                catch(SoapException e)
                {
                    Console.WriteLine("Error:");
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error:");
                    Console.WriteLine(e.Message);
                }
            }

            Start();
        }

        private static void TestENURegistrationCash()
        {
            var tcrHeaderType = GetTCRTestData();

            using (X509Certificate2 certificate = new X509Certificate2(KEYSTORE_LOCATION, KEYSTORE_PASS))
            {
                Fiscalization fiscalization = new Fiscalization(certificate);
                var response = fiscalization.RegisterTCR(tcrHeaderType);

                if (response != null)
                    Console.WriteLine("Registration successful!");
            }
        }

        private static void TestENURegistrationNoncash()
        {
            var tcrHeaderType = GetTCRTestData();

            using (X509Certificate2 certificate = new X509Certificate2(KEYSTORE_LOCATION, KEYSTORE_PASS))
            {
                Fiscalization fiscalization = new Fiscalization(certificate);
                var response = fiscalization.RegisterTCR(tcrHeaderType);

                if (response != null)
                    Console.WriteLine("Registration successful!");
            }
        }

        private static void TestRegisterCashDepositCash()
        {
            var cashDepositType = GetCashDepositTestData();

            using (X509Certificate2 certificate = new X509Certificate2(KEYSTORE_LOCATION, KEYSTORE_PASS))
            {
                Fiscalization fiscalization = new Fiscalization(certificate);
                var response = fiscalization.RegisterCashDeposit(cashDepositType);

                if (response != null)
                    Console.WriteLine("Registration successful!");
            }
        }

        private static void TestInvoiceRegistrationCash()
        {
            var cashDepositType = GetInvoiceTestDataCash();

            using (X509Certificate2 certificate = new X509Certificate2(KEYSTORE_LOCATION, KEYSTORE_PASS))
            {
                Fiscalization fiscalization = new Fiscalization(certificate);
                var response = fiscalization.RegisterInvoice(cashDepositType);

                if (response != null)
                    Console.WriteLine("Registration successful!");
            }
        }

        private static void TestInvoiceRegistrationNoncash()
        {
            var cashDepositType = GetInvoiceTestDataNoncash();

            using (X509Certificate2 certificate = new X509Certificate2(KEYSTORE_LOCATION, KEYSTORE_PASS))
            {
                Fiscalization fiscalization = new Fiscalization(certificate);
                var response = fiscalization.RegisterInvoice(cashDepositType);

                if (response != null)
                    Console.WriteLine("Registration successful!");
            }
        }

        private static TCRHeaderType GetTCRTestData()
        {
            return new TCRHeaderType()
            {
                Header = new RegisterTCRRequestHeaderType()
                {
                    UUID = Guid.NewGuid().ToString(),
                    SendDateTime = DateTime.Parse(DateTime.Now.ToString(Fiscalization.DATE_FORMAT_LONG))
                },
                TCR = new TCRType()
                {
                    IssuerTIN = "*******",
                    BusinUnitCode = "*******",
                    TCRIntID = "*******",
                    SoftCode = "*******",
                    MaintainerCode = "*******",
                    ValidFrom = DateTime.Parse(DateTime.Now.ToString(Fiscalization.DATE_FORMAT_SHORT)),
                    ValidFromSpecified = true,
                    Type = TCRSType.REGULAR,
                    TypeSpecified = true
                }
            };
        }

        private static CashDepositHeaderType GetCashDepositTestData()
        {
            return new CashDepositHeaderType()
            {
                Header = new RegisterCashDepositRequestHeaderType()
                {
                    UUID = Guid.NewGuid().ToString(),
                    SendDateTime = DateTime.Parse(DateTime.Now.ToString(Fiscalization.DATE_FORMAT_LONG))
                },
                CashDeposit = new CashDepositType()
                {
                    ChangeDateTime = DateTime.Parse(DateTime.Now.ToString(Fiscalization.DATE_FORMAT_LONG)),
                    Operation = CashDepositOperationSType.INITIAL,
                    CashAmt = 0.00m,
                    TCRCode = "*******",
                    IssuerTIN = "*******"
                }
            };
        }

        private static InvoiceHeaderType GetInvoiceTestDataCash()
        {
            return new InvoiceHeaderType()
            {
                IssuerTIN = "*******",
                Header = new RegisterInvoiceRequestHeaderType()
                {
                    UUID = Guid.NewGuid().ToString(),
                    SendDateTime = DateTime.Parse(DateTime.Now.ToString(Fiscalization.DATE_FORMAT_LONG))
                },
                Invoice = new InvoiceType()
                {
                    Seller = new SellerType()
                    {
                        IDType = IDTypeSType.TIN,
                        IDNum = "*******",
                        Name = "*******",
                        Address = "*******",
                        Town = "*******",
                        Country = CountryCodeSType.MNE,
                        CountrySpecified = true
                    },
                    Buyer = new BuyerType()
                    {
                        IDType = IDTypeSType.ID,
                        IDTypeSpecified = true,
                        IDNum = "*******",
                        Name = "*******",
                        Address = "*******",
                        Town = "*******",
                        Country = CountryCodeSType.MNE,
                        CountrySpecified = true
                    },
                    PayMethods = new[]
                    {
                        new PayMethodType()
                        {
                            Type=PaymentMethodTypeSType.BANKNOTE,
                            Amt=0.00m,
                        }
                    },
                    Items = new[]
                    {
                        new InvoiceItemType()
                        {
                            C="*******",
                            N="*******",
                            U="*******",
                            Q=0,
                            PA=0.0000m,
                            PB=0.0000m,
                            R=0,
                            RSpecified=true,
                            RR=true,
                            RRSpecified=true,
                            UPB=0.0000m,
                            UPA=0.0000m,
                            VA=0.0000m,
                            VASpecified=true,
                            VR=0.00m,
                            VRSpecified=true
                        }
                    },
                    SameTaxes = new[]
                    {
                        new SameTaxType()
                        {
                            NumOfItems=0,
                            PriceBefVAT=0.00m,
                            VATAmt=0.00m,
                            VATAmtSpecified=true,
                            VATRate=0.00m,
                            VATRateSpecified=true
                        }
                    },
                    TypeOfInv = InvoiceSType.CASH,
                    IsSimplifiedInv = false,
                    IssueDateTime = DateTime.Parse(DateTime.Now.ToString(Fiscalization.DATE_FORMAT_LONG)),
                    InvNum = "*******/*/****/*******",
                    InvOrdNum = 1,
                    TCRCode = "*******",
                    IsIssuerInVAT = true,
                    TotPriceWoVAT = 0.00m,
                    TotVATAmt = 0.00m,
                    TotVATAmtSpecified = true,
                    TotPrice = 0.00m,
                    OperatorCode = "*******",
                    BusinUnitCode = "*******",
                    SoftCode = "*******",
                    IsReverseCharge = false
                }
            };
        }

        private static InvoiceHeaderType GetInvoiceTestDataNoncash()
        {
            return new InvoiceHeaderType()
            {
                IssuerTIN = "*******",
                Header = new RegisterInvoiceRequestHeaderType()
                {
                    UUID = Guid.NewGuid().ToString(),
                    SendDateTime = DateTime.Parse(DateTime.Now.ToString(Fiscalization.DATE_FORMAT_LONG))
                },
                Invoice = new InvoiceType()
                {
                    Seller = new SellerType()
                    {
                        IDType = IDTypeSType.TIN,
                        IDNum = "*******",
                        Name = "*******",
                        Address = "*******",
                        Town = "*******",
                        Country = CountryCodeSType.MNE,
                        CountrySpecified = true
                    },
                    Buyer = new BuyerType()
                    {
                        IDType = IDTypeSType.ID,
                        IDTypeSpecified = true,
                        IDNum = "*******",
                        Name = "*******",
                        Address = "*******",
                        Town = "*******",
                        Country = CountryCodeSType.MNE,
                        CountrySpecified = true
                    },
                    PayMethods = new[]
                    {
                        new PayMethodType()
                        {
                            Type=PaymentMethodTypeSType.BANKNOTE,
                            Amt=0.00m,
                        }
                    },
                    Items = new[]
                    {
                        new InvoiceItemType()
                        {
                            C="*******",
                            N="*******",
                            U="*******",
                            Q=0,
                            PA=0.0000m,
                            PB=0.0000m,
                            R=0,
                            RSpecified=true,
                            RR=true,
                            RRSpecified=true,
                            UPB=0.0000m,
                            UPA=0.0000m,
                            VA=0.0000m,
                            VASpecified=true,
                            VR=0.00m,
                            VRSpecified=true
                        }
                    },
                    SameTaxes = new[]
                    {
                        new SameTaxType()
                        {
                            NumOfItems=0,
                            PriceBefVAT=0.00m,
                            VATAmt=0.00m,
                            VATAmtSpecified=true,
                            VATRate=0.00m,
                            VATRateSpecified=true
                        }
                    },
                    TypeOfInv = InvoiceSType.NONCASH,
                    IsSimplifiedInv = false,
                    IssueDateTime = DateTime.Parse(DateTime.Now.ToString(Fiscalization.DATE_FORMAT_LONG)),
                    InvNum = "*******/*/****/*******",
                    InvOrdNum = 1,
                    TCRCode = "*******",
                    IsIssuerInVAT = true,
                    TotPriceWoVAT = 0.00m,
                    TotVATAmt = 0.00m,
                    TotVATAmtSpecified = true,
                    TotPrice = 0.00m,
                    OperatorCode = "*******",
                    BusinUnitCode = "*******",
                    SoftCode = "*******",
                    IsReverseCharge = false
                }
            };
        }
    }
}
