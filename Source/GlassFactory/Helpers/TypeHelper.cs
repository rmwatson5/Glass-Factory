using System;

namespace SBR.GlassFactory.Helpers
{
    public static class TypeHelper
    {
        public static object ChangeType(object value, Type conversionType)
        {
            if (conversionType == typeof(Guid))
            {
                return new Guid(value.ToString());
            }

            return Convert.ChangeType(value, conversionType);
        }
    }
}
