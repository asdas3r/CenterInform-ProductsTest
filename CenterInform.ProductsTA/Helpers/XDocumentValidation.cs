using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Reflection;

namespace CenterInform.ProductsTA.Helpers
{
    public static class XDocumentValidation
    {
        public static string Validate(XDocument document, string xsdSchemaResourceStream)
        {
            try
            {
                XmlSchemaSet schemaSet = new XmlSchemaSet();

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(xsdSchemaResourceStream))
                {
                    XmlSchema xmlSchema = XmlSchema.Read(stream, null);
                    schemaSet.Add(xmlSchema);
                }

                string error = "";
                document.Validate(schemaSet, (o, e) =>
                {
                    error = "Ошибка валидации. Файл не соответствует представленной XSD-схеме: \n" + e.Message;
                });
                return error;
            }
            catch
            {
                return "Ошибка чтения файла!";
            }
        }
    }
}
