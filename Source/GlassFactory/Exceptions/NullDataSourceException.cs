using System;

namespace SBR.GlassFactory.Exceptions
{
    public class NullDataSourceException : NullReferenceException
    {
        public NullDataSourceException(Type dataSourceType)
            : base(FormatExceptionMessage(dataSourceType))
        {
            
        }

        protected static string FormatExceptionMessage(Type dataSourceType)
        {
            return $"Data Source of type {dataSourceType.Name} is required";
        }
    }
}
