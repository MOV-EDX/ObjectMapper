using ObjectMapper.Extensions;
using ObjectMapper.Mappers.Interfaces;

namespace ObjectMapper.Mappers
{
    public class SimpleMap : ISimpleMap
    {
        public TDest Map<TDest, TSource>(TSource source)
            where TDest : class
            where TSource : class
        {
            var destinationType = typeof(TDest);
            var sourceType = typeof(TSource);

            var destination = Activator.CreateInstance<TDest>();

            foreach (var property in destinationType.GetProperties())
            {
                var sourceProperty = sourceType.FindProperty(property);

                if (sourceProperty is not null)
                {
                    var sourceValue = sourceProperty.GetValue(source);
                    property.SetProperty(sourceValue!, destination!);
                }
            }

            return destination;
        }

        public IEnumerable<TDest> Map<TDest, TSource>(IEnumerable<TSource> source)
            where TDest : class
            where TSource : class
        {
            var destination = new List<TDest>(source.Count());

            foreach (var sourceItem in source)
            {
                var result = Map<TDest, TSource>(sourceItem);
                destination.Add(result);
            }

            return destination;
        }
    }
}
