namespace ObjectMapper.Extensions
{
    internal static class IDictionaryExtensions
    {
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
    }
}
