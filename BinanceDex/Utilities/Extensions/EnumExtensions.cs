using System;
using System.Linq;
using System.Reflection;

namespace BinanceDex.Utilities.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static int GetInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }
    }
}
