using System.Collections;
using System.Reflection;

namespace ObjectMapper.Extensions
{
    internal static class PropertyInfoExtensions
    {
        /// <summary>
        /// Checks if the property is a user defined class.
        /// </summary>
        /// <param name="property">The property to be checked.</param>
        /// <returns>A boolean which determines if the property is a class or not.</returns>
        public static bool IsClass(this PropertyInfo property)
        {
            return property.PropertyType.IsClass && !property.IsSystemType() && !property.IsEnumerable();
        }

        /// <summary>
        /// Checks if the property is an IEnumerable<T>
        /// </summary>
        /// <param name="property">The property to be checked.</param>
        /// <returns>A boolean which determines if the property is an IEnumerable<T> or not.</T></returns>
        public static bool IsEnumerable(this PropertyInfo property)
        {
            return property.PropertyType.GetInterface(nameof(IEnumerable)) is not null && !property.IsSystemType();
        }

        /// <summary>
        /// Checks if the property is a system defined type.
        /// </summary>
        /// <param name="property">The property to be checked.</param>
        /// <returns>A boolean which determines if the property is a system type or not.</returns>
        public static bool IsSystemType(this PropertyInfo property)
        {
            return property.PropertyType.FullName!.StartsWith("System") && !property.PropertyType.FullName.StartsWith("System.Collections");
        }
    }
}
