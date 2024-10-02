namespace ObjectMapper.Mappers.Interfaces
{
    public interface IRecursiveMap
    {
        public TDest Map<TDest, TSource>(TSource source);
    }
}
