# ObjectMapper

To register using dependency injection, you can use the following:

``builder.Services.AddObjectMapper();``

This simply registers the mapping service as a singleton. The mapper will map between compatible types and will either map
based on the property name or the [Alias(Name = "PropertyName")] attribute applied to the target property in the destination object. This is
done recursively for each composite object, including those used on generic types like IEnumerable<T>.

The usage is very simple, once the IMapper interface has been injected into your consuming class, then you will just need to do the following to map between
the two objects.

``TDest newObject = _mapper.Map<TDest, TSource>(TSource object);``
