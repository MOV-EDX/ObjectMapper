# ObjectMapper

To register using dependency injection, you can use the following:

``builder.Services.AddObjectMapper();``

This simply registers the mapping service as a singleton. The mapper will map between compatible types and will either map
based on the property name or the [Alias(Name = "PropertyName")] attribute applied to the target property in the destination object.
