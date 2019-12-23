namespace CenterInform.ProductsTA.Interfaces
{
    public interface IFileService
    {
        string ErrorString { get; set; }

        T GetFromFile<T>(string filePath);

        bool SaveToFile<T>(string filePath, T serializedObject);
    }
}
