namespace CenterInform.ProductsTA.Interfaces
{
    public interface IObjectUsageControlService<T>
    {
        void SetUsed(T obj);
        
        void UnsetUsed(T obj);

        bool CheckIfUsed(T obj);
    }
}
