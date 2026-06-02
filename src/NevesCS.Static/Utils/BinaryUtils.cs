using System.ComponentModel;
using System.Reflection;
using System.Text;

using NevesCS.Static.Constants;
using NevesCS.Static.Constants.Values;

namespace NevesCS.Static.Utils
{
    public static class BinaryUtils
    {
        /// <summary>
        /// Serializes primitive type properties of an object.
        ///
        /// </summary>
        public static byte[] SerializeObject<T>(T target)
        {
            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream);

            foreach (var property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).ToArray())
            {
                var value = property.GetValue(target);

                if (value == null)
                {
                    continue;
                }

                writer.Write($"{property.Name}{Chars.Equal}{value}");
            }

            memoryStream.Flush();

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Deserializes primitive type properties of a previously serialized object.
        ///
        /// </summary>
        public static T DeserializeByteObject<T>(byte[] serializedData)
            where T : class, new()
        {
            var target = new T();
            var memoryStream = new MemoryStream(serializedData);
            using var reader = new BinaryReader(memoryStream, Encoding.UTF8);

            var allPropertyInfos = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop);

            var typeConverterCache = new Dictionary<Type, TypeConverter>();

            while (reader.PeekChar() != -Ints.One)
            {
                var currentPropAndValue = reader.ReadString().Split(Chars.Equal);

                if (!allPropertyInfos.TryGetValue(currentPropAndValue[Ints.Zero], out PropertyInfo? propertyInfo))
                {
                    continue;
                }

                object? value = string.Concat(currentPropAndValue.Skip(Ints.One).ToArray());

                if (propertyInfo.PropertyType != TypeOf.String)
                {
                    var converter = DictionaryUtils.GetOrCreate(
                        typeConverterCache,
                        propertyInfo.PropertyType,
                        () => TypeDescriptor.GetConverter(propertyInfo.PropertyType));

                    if (!converter.CanConvertFrom(TypeOf.String))
                    {
                        //throw new NotImplementedException($"Cannot parse property type of {propertyInfo.PropertyType.Name} (not implemented).");

                        continue;
                    }

                    value = converter.ConvertFromString(currentPropAndValue[Ints.One]);
                }

                propertyInfo!.SetValue(target, value);
            }

            return target;
        }
    }
}
