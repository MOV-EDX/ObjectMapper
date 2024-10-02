using System.Reflection;
using ObjectMapper.Attributes;

namespace ObjectMapper.Extensions
{
    internal static class PropertyMapper
    {
        internal static void SetProperty(this PropertyInfo property, object value, object destination)
        {
            var destinationName = destination.GetType().Name;
            var valueType = value.GetType();

            if (valueType.IsAssignableTo(property.PropertyType))
            {
                var typedValue = Convert.ChangeType(value, property.PropertyType);
                property.SetValue(destination, typedValue);
                return;
            }

            throw new InvalidCastException($"Unable to assign {value} to {property.Name} for {destinationName}");
        }

        internal static PropertyInfo FindProperty(this Type source, PropertyInfo destination)
        {
            var destinationAlias = destination.GetCustomAttribute<AliasAttribute>();
            var propertyToFind = destinationAlias is not null ? destinationAlias.Name : destination.Name;

            // Search for the source property using the alias
            var sourceProperty = source.GetProperty(propertyToFind);

            if (sourceProperty is not null) return sourceProperty;

            // Fallback and use the original property name
            return source.GetProperty(destination.Name)!;
        }

        internal static object GetPropertyValue(this PropertyInfo property, object source)
        {
            return property.GetValue(source, null)!;
        }
    }
}
