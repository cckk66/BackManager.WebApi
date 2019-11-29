using System;
using System.Collections.Generic;
using System.Text;

namespace BackManager.Utility.Tool
{
    internal static class UtilConstants
    {
     


        internal static Type IntType = typeof(int);
        internal static Type LongType = typeof(long);
        internal static Type GuidType = typeof(Guid);
        internal static Type BoolType = typeof(bool);
        internal static Type BoolTypeNull = typeof(bool?);
        internal static Type ByteType = typeof(Byte);
        internal static Type ObjType = typeof(object);
        internal static Type DobType = typeof(double);
        internal static Type FloatType = typeof(float);
        internal static Type ShortType = typeof(short);
        internal static Type DecType = typeof(decimal);
        internal static Type StringType = typeof(string);
        internal static Type DateType = typeof(DateTime);
        internal static Type DateTimeOffsetType = typeof(DateTimeOffset);
        internal static Type TimeSpanType = typeof(TimeSpan);
        internal static Type ByteArrayType = typeof(byte[]);
        internal static Type Dicii = typeof(KeyValuePair<int, int>);
        internal static Type DicIS = typeof(KeyValuePair<int, string>);
        internal static Type DicSi = typeof(KeyValuePair<string, int>);
        internal static Type DicSS = typeof(KeyValuePair<string, string>);
        internal static Type DicOO = typeof(KeyValuePair<object, object>);
        internal static Type DicSo = typeof(KeyValuePair<string, object>);
        internal static Type DicArraySS = typeof(Dictionary<string, string>);
        internal static Type DicArraySO = typeof(Dictionary<string, object>);

    }
}
