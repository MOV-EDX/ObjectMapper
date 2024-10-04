using System.Collections;
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
                        var populatedProperty = Map(sourceValue!, property.PropertyType);
                        property.SetProperty(populatedProperty!, destination!);
                    }

                    if (property.IsEnumerable())
                    {
                        // Check wherever the IEnumerable is an array or IEnumerable<T>
                        var isArray = sourceProperty.PropertyType.IsArray;

                        var sourceEnumerable = sourceValue as IEnumerable;
                        var sourceEnumerableValue = sourceEnumerable!.Cast<object>();

                        var length = sourceEnumerableValue?.Count() ?? 0;

                        object valueToSet = null!;
                        var elementType = isArray ? property.PropertyType.GetElementType() : property.PropertyType.GenericTypeArguments[0];

                        // Check if the element type is a class
                        var isClass = elementType!.IsClass();

                        if (isClass)
                        {
                            var targetEnumerable = new List<object>(length);

                            foreach (var element in sourceEnumerableValue!)
                            {
                                var populatedElement = Map(element!, elementType!);
                                targetEnumerable.Add(populatedElement);
                            }

                            valueToSet = isArray ? targetEnumerable.ToTypedArray(elementType!) : targetEnumerable.ToTypedEnumerable(elementType!);
                        }
                        else
                        {
                            valueToSet = isArray ? sourceEnumerableValue!.ToTypedArray(elementType!) : sourceEnumerableValue!.ToTypedEnumerable(elementType!);
                        }

                        property.SetEnumerableProperty(valueToSet!, destination!);
                    }
                    else
                    {
                        property.SetProperty(sourceValue!, destination!);
                    }
                }
            }

            return destination!;
        }
    }
}
