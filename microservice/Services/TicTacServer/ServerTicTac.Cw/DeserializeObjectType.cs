using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace ServerTicTac.Cw
{
    public class DeserializeObjectType
    {
        public static T DeserializeObjectWithInspectingType<T>(byte[] buffer, int bytesRead)
        {
            using (MemoryStream memoryStream = new MemoryStream(buffer, 0, bytesRead))
            {
                T deserializedObject = Serializer.Deserialize<T>(memoryStream);
                return deserializedObject;
            }
        }
        public static byte[] SerializeObjectWithInspectingType<T>(T obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Serializer.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        } 
        public static byte[] SerializeListObjectWithInspectingType<T>(IEnumerable<T> obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Serializer.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }
    }
}
