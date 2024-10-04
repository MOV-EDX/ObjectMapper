namespace ObjectMapper.Mappers.Interfaces
{
    public interface IMapper
    {
        /// <summary>
        /// Maps the source type to the destination type using an instance of source. This mapper
        /// will map on property names and assigned aliases.
        /// </summary>
        /// <typeparam name="TDest">The type to map to</typeparam>
        /// <typeparam name="TSource">The type to map from</typeparam>
        /// <param name="source">An instance of the source type</param>
        /// <returns>An instance of the destination type with populated properties</returns>
        public TDest Map<TDest, TSource>(TSource source)
            where TDest : class
            where TSource : class;

        /// <summary>
        /// Maps an IEnumerable to another IEnumerable using the property names and aliases of
        /// the generic type arguments.
        /// </summary>
        /// <typeparam name="TDest">The type to map to</typeparam>
        /// <typeparam name="TSource">The type to map from</typeparam>
        /// <param name="source">An instance of the source collection</param>
        /// <returns>An instance of IEnumerable<TDest> with populated elements</returns>
        public IEnumerable<TDest> Map<TDest, TSource>(IEnumerable<TSource> source)
            where TDest : class
            where TSource : class;
    }
}
