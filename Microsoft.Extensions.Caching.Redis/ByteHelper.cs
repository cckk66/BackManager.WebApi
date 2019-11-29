using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Caching.Redis
{
    public static class ByteHelper
    {
        public static byte[] ToBytes<T>(object t)
        {
            return t == null ? throw new Exception("传入值为空") : GetBytes(t);
        }

        public static byte[] GetBytes(object t)
        {
            return t == null ? throw new Exception("传入值为空") : Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(t));
        }

        public static T ToModel<T>(byte[] Bytes)
        {
            return Bytes == null ? throw new Exception("传入值为空") : JsonConvert.DeserializeObject<T>(Bytes.CToString());
        }

        public static List<T> ToList<T>(byte[] Bytes)
        {
            return Bytes == null ? throw new Exception("传入值为空") : JsonConvert.DeserializeObject<List<T>>(Bytes.CToString());
        }

        public static string CToString(this byte[] Bytes)
        {
            return Bytes == null ? throw new Exception("传入值为空") : System.Text.Encoding.Default.GetString(Bytes);
        }
    }
}
















