namespace ObjectMapper.Extensions
{
    internal static class EnumerableExtensions
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

        public static object ToTypedDictionary(this IDictionary<object, object> input, Type key, Type value)
        {
            var targetType = typeof(Dictionary<,>);
            var dictionary = targetType.MakeGenericType(key, value);

            var constructor = dictionary.GetConstructor(new Type[] { typeof(int) });
            var result = constructor?.Invoke(new object[] { input.Count() });

            // Map the objects from the untyped enumerable to the generic type version
            foreach (var item in input)
            {
                _ = result?.GetType()?.GetMethod("Add")?.Invoke(result, new object[] { item.Key, item.Value });
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
