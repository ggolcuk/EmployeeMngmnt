using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities.EnumUtilities
{
    public static class EnumUtility
    {
        /// <summary>
        /// Get a description for the given enum value
        /// </summary>
        /// <param name="value">Th enum value</param>
        /// <returns>A description corresponding to the value's Description attribute if defined, or the value's name otherwise</returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false
                );

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// Get the enum value of the given type corresponding to the given description
        /// </summary>
        /// <param name="description">The description of the value</param>
        /// <param name="enumType">The type of the enum</param>
        /// <returns>Th enum value corresponding to the given description</returns>
        public static Enum FromDescription(string description, Type enumType)
        {
            Array values = Enum.GetValues(enumType);
            foreach (Enum value in values)
            {
                try
                {
                    string valueDesc = GetDescription(value);
                    if (valueDesc == description)
                        return value;
                }
                catch
                {/*pass*/
                }

                //FieldInfo fi = description.GetType().GetField(description);
                //DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                //                                                                typeof(DescriptionAttribute),
                //                                                                false
                //                                                                );
                //if (attributes.Length > 0 && attributes[0].Description == description)
                //    return value;
            }
            return (Enum)Enum.Parse(enumType, description);
        }

        /// <summary>
        /// Parses the textual value of an enum to get its value
        /// </summary>
        /// <typeparam name="T">The type of the enum</typeparam>
        /// <param name="value">The enum textual value</param>
        /// <returns>The value corresponding to the textual value</returns>
        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Get all the values for the given enum type
        /// </summary>
        /// <typeparam name="T">The type of the enum</typeparam>
        /// <returns>All the values defined for the given enum type</returns>
        public static IList<T> GetValues<T>()
        {
            IList<T> list = new List<T>();
            foreach (object value in Enum.GetValues(typeof(T)))
            {
                list.Add((T)value);
            }
            return list;
        }

        /// <summary>
        /// Get all the descriptions for the given enum type
        /// </summary>
        /// <typeparam name="T">The type of the enum</typeparam>
        /// <returns>For each enum value, a description corresponding to the value's Description attribute if defined, or the value's name otherwise</returns>
        public static IList<string> GetDescriptions<T>()
        {
            IList<string> list = new List<string>();
            foreach (Enum value in Enum.GetValues(typeof(T)))
            {
                list.Add(GetDescription(value));
            }
            return list;
        }
    }
}
