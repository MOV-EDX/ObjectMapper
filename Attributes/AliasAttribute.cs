namespace ObjectMapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class AliasAttribute : Attribute
    {
        public AliasAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
