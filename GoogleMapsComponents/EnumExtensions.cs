using System;

namespace GoogleMapsComponents
{
    internal static class EnumExtensions
    {
        public static string? GetMemberAttr(this Enum enumItem)
        {
            var memberInfo = enumItem.GetType().GetMember(enumItem.ToString());
            if (memberInfo.Length == 0) { return null; }

            foreach (var attr in memberInfo[0].GetCustomAttributes(false))
            {
                if (attr is System.Runtime.Serialization.EnumMemberAttribute val)
                {
                    return val.Value;
                }
            }
            return null;
        }
    }
}
