using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace cs_lab_serializacja_szyfrowanie_GraZgadywanka.src
{
    public static class BinarySerialization
    {
        public static byte[] SerializeToMemory<T> (T obj)
        {
            using var memStream = new MemoryStream();
            var formatter = new BinaryFormatter();

            formatter.Serialize(memStream, obj);
            return memStream.ToArray();
        }

        public static T DeserializeFromMemory<T>(byte[] streamObj)
        {
            using var memStream = new MemoryStream(streamObj);
            var formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(memStream);
        }
    }
}
