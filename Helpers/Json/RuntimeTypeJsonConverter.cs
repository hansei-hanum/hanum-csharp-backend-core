
using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hanum.Core.Helpers.Json;

/// <summary>
/// Instructs the JsonSerializer to serialize an object as its runtime type and not the type parameter passed into the Write function.
/// </summary>
public class RuntimeTypeJsonConverter<T> : JsonConverter<T> {
    private static readonly Dictionary<Type, PropertyInfo[]> _knownProps = []; //cache mapping a Type to its array of public properties to serialize
    private static readonly Dictionary<Type, JsonConverter> _knownConverters = []; //cache mapping a Type to its respective RuntimeTypeJsonConverter instance that was created to serialize that type. 
    private static readonly Dictionary<Type, Type> _knownGenerics = []; //cache mapping a Type to the type of RuntimeTypeJsonConverter generic type definition that was created to serialize that type

    public override bool CanConvert(Type typeToConvert) {
        return typeToConvert.IsClass && typeToConvert != typeof(string); //this converter is only meant to work on reference types that are not strings
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var deserialized = JsonSerializer.Deserialize(ref reader, typeToConvert, options); //default read implementation, the focus of this converter is the Write operation
        return (T)deserialized!;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
        if (value is IEnumerable) //if the value is an IEnumerable of any sorts, serialize it as a JSON array. Note that none of the properties of the IEnumerable are written, it is simply iterated over and serializes each object in the IEnumerable
        {
            RuntimeTypeJsonConverter<T>.WriteIEnumerable(writer, value, options);
        } else if (value != null && value.GetType().IsClass == true) //if the value is a reference type and not null, serialize it as a JSON object.
          {
            RuntimeTypeJsonConverter<T>.WriteObject(writer, value, ref options);
        } else //otherwise just call the default serializer implementation of this Converter is asked to serialize anything not handled in the other two cases
          {
            JsonSerializer.Serialize(writer, value, options);
        }
    }

    /// <summary>
    /// Writes the values for an object into the Utf8JsonWriter
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to Json.</param>
    /// <param name="options">An object that specifies the serialization options to use.</param>
    private static void WriteObject(Utf8JsonWriter writer, T value, ref JsonSerializerOptions options) {
        var type = value!.GetType();

        //get all the public properties that we will be writing out into the object
        PropertyInfo[] props = RuntimeTypeJsonConverter<T>.GetPropertyInfos(type);

        writer.WriteStartObject();

        foreach (var prop in props) {
            var propVal = prop.GetValue(value);
            if (propVal == null) continue; //don't include null values in the final graph

            writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(prop.Name) ?? prop.Name);
            var propType = propVal.GetType(); //get the runtime type of the value regardless of what the property info says the PropertyType should be

            if (propType.IsClass && propType != typeof(string)) //if the property type is a valid type for this JsonConverter to handle, do some reflection work to get a RuntimeTypeJsonConverter appropriate for the sub-object
            {
                Type generic = RuntimeTypeJsonConverter<T>.GetGenericConverterType(propType); //get a RuntimeTypeJsonConverter<T> Type appropriate for the sub-object
                JsonConverter converter = RuntimeTypeJsonConverter<T>.GetJsonConverter(generic); //get a RuntimeTypeJsonConverter<T> instance appropriate for the sub-object

                //look in the options list to see if we don't already have one of these converters in the list of converters in use (we may already have a converter of the same type, but it may not be the same instance as our converter variable above)
                var found = false;
                foreach (var converterInUse in options.Converters) {
                    if (converterInUse.GetType() == generic) {
                        found = true;
                        break;
                    }
                }

                if (found == false) //not in use, make a new options object clone and add the new converter to its Converters list (which is immutable once passed into the Serialize method).
                {
                    options = new JsonSerializerOptions(options);
                    options.Converters.Add(converter);
                }

                JsonSerializer.Serialize(writer, propVal, propType, options);
            } else //not one of our sub-objects, serialize it like normal
              {
                JsonSerializer.Serialize(writer, propVal, options);
            }
        }

        writer.WriteEndObject();
    }

    /// <summary>
    /// Gets or makes RuntimeTypeJsonConverter generic type to wrap the given type parameter.
    /// </summary>
    /// <param name="propType">The type to get a RuntimeTypeJsonConverter generic type for.</param>
    /// <returns></returns>
    private static Type GetGenericConverterType(Type propType) {
        Type generic;
        if (_knownGenerics.ContainsKey(propType) == false) {
            generic = typeof(RuntimeTypeJsonConverter<>).MakeGenericType(propType);
            _knownGenerics.Add(propType, generic);
        } else {
            generic = _knownGenerics[propType];
        }

        return generic;
    }

    /// <summary>
    /// Gets or creates the corresponding RuntimeTypeJsonConverter that matches the given generic type defintion.
    /// </summary>
    /// <param name="genericType">The generic type definition of a RuntimeTypeJsonConverter.</param>
    /// <returns></returns>
    private static JsonConverter GetJsonConverter(Type genericType) {
        JsonConverter converter;
        if (_knownConverters.ContainsKey(genericType) == false) {
            converter = (JsonConverter)Activator.CreateInstance(genericType)!;
            _knownConverters.Add(genericType, converter);
        } else {
            converter = _knownConverters[genericType];
        }

        return converter;
    }



    /// <summary>
    /// Gets all the public properties of a Type.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private static PropertyInfo[] GetPropertyInfos(Type t) {
        PropertyInfo[] props;
        if (_knownProps.ContainsKey(t) == false) {
            props = t.GetProperties();
            _knownProps.Add(t, props);
        } else {
            props = _knownProps[t];
        }

        return props;
    }

    /// <summary>
    /// Writes the values for an object that implements IEnumerable into the Utf8JsonWriter
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to Json.</param>
    /// <param name="options">An object that specifies the serialization options to use.</param>
    private static void WriteIEnumerable(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
        writer.WriteStartArray();

        foreach (object item in (IEnumerable)value!) {
            if (item == null) //preserving null gaps in the IEnumerable
            {
                writer.WriteNullValue();
                continue;
            }

            JsonSerializer.Serialize(writer, item, item.GetType(), options);
        }

        writer.WriteEndArray();
    }
}