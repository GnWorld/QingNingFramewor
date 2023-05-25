using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingNing.Framework
{
    public static class EnumHelper<T> where T : struct
    {
        // 获取枚举字段名称
        public static string GetEnumFieldName(T value)
        {
            return Enum.GetName(typeof(T), value);
        }

        // 根据整数获取枚举值
        public static T GetEnumValue(int intValue)
        {
            return (T)Enum.ToObject(typeof(T), intValue);
        }

        // 根据字符串获取枚举值
        public static T GetEnumValue(string stringValue)
        {
            return (T)Enum.Parse(typeof(T), stringValue, true);
        }

        // 根据字符串转换为枚举
        public static bool TryParseEnum(string stringValue, out T enumValue)
        {
            return Enum.TryParse(stringValue, true, out enumValue);
        }

        // 根据值获取枚举
        public static T GetEnumFromValue(object value)
        {
            foreach (T enumItem in Enum.GetValues(typeof(T)))
            {
                if (enumItem.Equals(value))
                {
                    return enumItem;
                }
            }

            throw new ArgumentException($"Value '{value}' does not exist in enum {typeof(T)}.");
        }
    }
}
