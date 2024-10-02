using System.Collections;
using System.Reflection;

namespace ObjectMapper.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static bool IsClass(this PropertyInfo property)
        {
            return property.PropertyType.IsClass && !property.IsSystemType() && !property.IsEnumerable();
        }

        public static bool IsEnumerable(this PropertyInfo property)
        {
            return property.PropertyType.GetInterface(nameof(IEnumerable)) is not null && !property.IsSystemType();
        }

        public static bool IsSystemType(this PropertyInfo property)
        {
            return property.PropertyType.FullName!.StartsWith("System") && !property.PropertyType.FullName.StartsWith("System.Collections");
        }
    }
}
