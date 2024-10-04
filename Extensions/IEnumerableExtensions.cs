namespace ObjectMapper.Extensions
{
    internal static class IEnumerableExtensions
    {
        public static object ToTypedEnumerable(this IEnumerable<object> input, Type target)
        {
            // Define a new generic type using the target type
            var targetType = typeof(List<>);
            var enumerable = targetType.MakeGenericType(target);

            // Construct the newly defined list
            var constructor = enumerable.GetConstructor(new Type[] { typeof(int) });
            var result = constructor?.Invoke(new object[] { input.Count() });

            // Map the objects from the untyped enumerable to the generic type version
            foreach (var item in input)
            {
                _ = result?.GetType()?.GetMethod("Add")?.Invoke(result, new object[] { item });
            }

            return result!;
        }

        public static object ToTypedArray(this IEnumerable<object> input, Type target)
        {
            // Define a new generic type using the target type
            var targetType = typeof(List<>);
            var enumerable = targetType.MakeGenericType(target);

            // Construct the newly defined list
            var constructor = enumerable.GetConstructor(new Type[] { typeof(int) });
            var result = constructor?.Invoke(new object[] { input.Count() });

            // Map the objects from the untyped enumerable to the generic type version
            foreach (var item in input)
            {
                _ = result?.GetType()?.GetMethod("Add")?.Invoke(result, new object[] { item });
            }

            var array = result?.GetType().GetMethod("ToArray")?.Invoke(result, new object[] { });

            return array!;
        }
    }
}
