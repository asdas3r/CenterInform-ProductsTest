using System.Xml;
using System.Xml.Linq;

using CenterInform.ProductsTA.Interfaces;
using CenterInform.ProductsTA.Helpers;

namespace CenterInform.ProductsTA.Services
{
    class XmlService : IFileService
    {
        public string ErrorString { get; set; }

        public T GetFromFile<T>(string filePath)
        {
            XDocument doc = XDocument.Load(XmlReader.Create(filePath));

            ErrorString = XDocumentValidation.Validate(doc, "CenterInform.ProductsTA.Resources.ProductsSchema.xsd");

            if (!string.IsNullOrEmpty(ErrorString))
            {
                return default;
            }

            return XDocumentSerialization.Deserialize<T>(doc);
        }

        public bool SaveToFile<T>(string filePath, T serializedObject)
        {
            XDocument doc = XDocumentSerialization.Serialize(serializedObject);

            if (!string.IsNullOrEmpty(ErrorString))
            {
                return false;
            }

            doc.Save(filePath);
            return true;
        }
    }
}
