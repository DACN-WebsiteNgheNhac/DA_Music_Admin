namespace StartUpHelperWPF
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }
}