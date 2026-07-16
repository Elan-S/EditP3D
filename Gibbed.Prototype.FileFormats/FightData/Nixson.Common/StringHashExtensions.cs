namespace Nixson.Common
{
    public static class StringHashExtensions
    {
        public static uint HashFileName(this string value)
        {
            return Utils.RCFStringHash((value ?? string.Empty).TrimEnd('\0'));
        }
    }
}
