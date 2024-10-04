using System.Collections;
using System.Reflection;
using ObjectMapper.Extensions;
using ObjectMapper.Mappers.Interfaces;

namespace ObjectMapper.Mappers
{
    public sealed class RecursiveMap : IMapper
    {
        public TDest Map<TDest, TSource>(TSource source)
            where TSource : class
            where TDest : class
        {
            var destinationType = typeof(TDest);
            var sourceType = typeof(TSource);

            var destination = Map(source!, destinationType);

            return (TDest)destination;
        }

        public IEnumerable<TDest> Map<TDest, TSource>(IEnumerable<TSource> source)
            where TSource : class
            where TDest : class
        {
            var destination = new List<TDest>(source.Count());

            foreach (var sourceItem in source)
            {
                var result = Map<TDest, TSource>(sourceItem);
                destination.Add(result);
            }

            return destination;
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

                    if (property.PropertyType.IsEnum)
                    {
                        var convertedEnum = Enum.ToObject(property.PropertyType, sourceValue!);
                        property.SetProperty(convertedEnum!, destination!);
                        continue;
                    }

                    // Convert from DateTime to DateTimeOffset
                    if (property.IsDateTimeOffset() && sourceProperty.IsDateTime())
                    {
                        var dateTime = (DateTime)sourceValue!;
                        var dateTimeOffset = dateTime.ToDateTimeOffset();
                        property.SetProperty(dateTimeOffset!, destination!);
                        continue;
                    }

                    // Convert from DateTimeOffset to DateTime
                    if (property.IsDateTime() && sourceProperty.IsDateTimeOffset())
                    {
                        var dateTimeOffset = (DateTimeOffset)sourceValue!;
                        var dateTime = dateTimeOffset.ToDateTime();
                        property.SetProperty(dateTime!, destination!);
                        continue;
                    }

                    if (property.IsClass())
                    {
                        var populatedProperty = Map(sourceValue!, property.PropertyType);
                        property.SetProperty(populatedProperty!, destination!);
                        continue;
                    }

                    if (property.IsDictionary())
                    {
                        var targetDictionary = MapDictionaryProperty(sourceValue!, property);
                        property.SetEnumerableProperty(targetDictionary, destination!);
                        continue;
                    }

                    if (property.IsEnumerable())
                    {
                        // Check wherever the IEnumerable is an array or IEnumerable<T>
                        var isArray = sourceProperty.PropertyType.IsArray;
                        var valueToSet = MapEnumerableProperty(sourceValue!, property, isArray);

                        property.SetEnumerableProperty(valueToSet!, destination!);
                        continue;
                    }

                    property.SetProperty(sourceValue!, destination!);
                }
            }

            return destination!;
        }

        private object CreateTypedEnumerableOrArray(IEnumerable<object> source, Type elementType, bool isArray)
        {
            return isArray ? source!.ToTypedArray(elementType!) : source!.ToTypedEnumerable(elementType!);
        }

        private object MapEnumerableProperty(object sourceValue, PropertyInfo property, bool isArray)
        {
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

                valueToSet = CreateTypedEnumerableOrArray(targetEnumerable, elementType!, isArray);
            }
            else
            {
                valueToSet = CreateTypedEnumerableOrArray(sourceEnumerableValue!, elementType!, isArray);
            }

            return valueToSet;
        }

        private object MapDictionaryProperty(object sourceValue, PropertyInfo property)
        {
            var sourceDictionary = sourceValue as IDictionary;
            var targetDictionary = new Dictionary<object, object>();

            // Get the target key and value types to create the key value pair for
            var keyType = property.PropertyType.GenericTypeArguments[0];
            var valueType = property.PropertyType.GenericTypeArguments[1];

            foreach (DictionaryEntry pair in sourceDictionary!)
            {
                object key = null!;
                object value = null!;

                if (keyType.IsSystemType())
                {
                    key = Convert.ChangeType(pair.Key, keyType);
                }
                else
                {
                    key = Map(pair.Key!, keyType);
                }

                if (valueType.IsSystemType())
                {
                    value = Convert.ChangeType(pair.Value, valueType)!;
                }
                else
                {
                    value = Map(pair.Value!, valueType);
                }

                targetDictionary.Add(key, value);
            }

            return targetDictionary.ToTypedDictionary(keyType, valueType);
        }
    }
}
