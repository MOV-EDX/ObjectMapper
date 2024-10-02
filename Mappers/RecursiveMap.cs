using ObjectMapper.Extensions;
using ObjectMapper.Mappers.Interfaces;

namespace ObjectMapper.Mappers
{
    public class RecursiveMap : IRecursiveMap
    {
        public TDest Map<TDest, TSource>(TSource source)
        {
            var destinationType = typeof(TDest);
            var sourceType = typeof(TSource);

            var destination = Map(source!, destinationType);

            return (TDest)destination;
        }

        private object Map(object source, Type destinationType)
        {
            var sourceType = source.GetType();
            var destination = Activator.CreateInstance(destinationType);

            foreach (var property in destinationType.GetProperties())
            {
                var sourceProperty = sourceType.FindProperty(property);

                if (sourceProperty is not null)
                {
                    var sourceValue = sourceProperty.GetValue(source);

                    if (property.IsClass())
                    {
                        var populatedProperty = Map(source!, destinationType);
                        property.SetProperty(populatedProperty!, destination!);
                    }
                    else
                    {
                        var populatedProperty = Map(source!, destinationType);
                        property.SetProperty(populatedProperty!, destination!);
                    }
                }
            }

            return destination!;
        }
    }
}
