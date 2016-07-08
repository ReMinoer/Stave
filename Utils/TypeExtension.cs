using System;
using System.Linq;

namespace Stave.Utils
{
    static public class TypeExtension
    {
        static public string GetDisplayName(this Type type)
        {
            string result = type.Name;

            if (type.IsGenericType)
            {
                result = result.Substring(0, result.Length - 2);
                result += "<";
                result += string.Join(",", type.GenericTypeArguments.Select(GetDisplayName));
                result += ">";
            }

            return result;
        }
    }
}