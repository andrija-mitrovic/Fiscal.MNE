using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Fiscal.MNE.Helpers
{
    internal class SerializationHelper
    {
        private const string NAMESPACE = "https://efi.tax.gov.me/fs/schema";

        public static string SerializeDataToXmlString<T>(T request)
        {
            using (var ms = new MemoryStream())
            {
                var root = new XmlRootAttribute { Namespace = NAMESPACE, IsNullable = false };
                var serializer = new XmlSerializer(request.GetType(), root);
                serializer.Serialize(ms, request);

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
