using System.Collections;

namespace ObjectMapper.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsSystemType(this Type type)
        {
            return type.FullName!.StartsWith("System") && !type.FullName.StartsWith("System.Collections");
        }

        public static bool IsEnumerable(this Type type)
        {
            return type.GetInterface(nameof(IEnumerable)) != null && !type.IsSystemType();
        }

        public static bool IsClass(this Type type)
        {
            return type.IsClass && !type.IsSystemType() && !type.IsEnumerable();
        }
    }
}
