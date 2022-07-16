namespace Bluefish.Blazor.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsBool(this Type type) => type == typeof(Boolean)
            || type == typeof(Boolean?);

        public static bool IsDate(this Type type) => type == typeof(DateTime)
            || type == typeof(DateTime?)
            || type == typeof(DateTimeOffset)
            || type == typeof(DateTimeOffset?);

        public static bool IsFloat(this Type type) => type == typeof(Decimal)
            || type == typeof(Decimal?)
            || type == typeof(Single)
            || type == typeof(Single?)
            || type == typeof(Double)
            || type == typeof(Double?);

        public static bool IsGuid(this Type type) => type == typeof(Guid)
            || type == typeof(Guid?);

        public static bool IsInteger(this Type type) => type == typeof(Int16)
            || type == typeof(Int16?)
            || type == typeof(Int32)
            || type == typeof(Int32?)
            || type == typeof(Int64)
            || type == typeof(Int64?)
            || type == typeof(UInt16)
            || type == typeof(UInt16?)
            || type == typeof(UInt32)
            || type == typeof(UInt32?)
            || type == typeof(UInt64)
            || type == typeof(UInt64?);

        public static bool IsNullable(this Type type) => type == typeof(String)
            || type.FullName.StartsWith("Nullable");

        public static bool IsText(this Type type) => type == typeof(String);
    }
}
