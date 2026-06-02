using System.ComponentModel;

namespace NevesCS.Static.Utils
{
    public static class EnumUtils
    {
        public static string GetDescription<T>(T enumValue)
            where T : Enum
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());

            if (field is null || Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute attribute)
            {
                throw new ArgumentException($"{nameof(DescriptionAttribute)} not found.", nameof(enumValue));
            }

            return attribute.Description;
        }

        public static T FromName<T>(string name)
            where T : Enum
        {
            return (T)Enum.Parse(typeof(T), name);
        }

        public static bool TryFromName<T>(string name, out T? value)
            where T : Enum
        {
            var success = Enum.TryParse(typeof(T), name, out object? result);

            if (success)
            {
                value = (T?)result;
            }
            else
            {
                value = default;
            }

            return success;
        }

        public static T FromDescription<T>(string description, bool checkFieldName = true)
            where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null)!;
                    }
                }
                else
                {
                    if (checkFieldName && field.Name == description)
                    {
                        return (T)field.GetValue(null)!;
                    }
                }
            }

            throw new ArgumentException($"{nameof(DescriptionAttribute)} not found.", nameof(description));
        }
    }
}
