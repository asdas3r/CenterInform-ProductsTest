using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

using CenterInform.ProductsTA.Interfaces;

namespace CenterInform.ProductsTA.Helpers
{
    public static class XDocumentSerialization
    {
        public static T Deserialize<T>(XDocument document)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (var reader = document.CreateReader())
            {
                var deserializedObject = (T)xmlSerializer.Deserialize(reader);
                return deserializedObject;
            }
        }

        public static XDocument Serialize<T>(T serializedObject)
        {
            XDocument doc = new XDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
            xmlns.Add(string.Empty, string.Empty);

            using (var writer = doc.CreateWriter())
            {
                xmlSerializer.Serialize(writer, serializedObject, xmlns);
            }

            return doc;
        }
    }
}
