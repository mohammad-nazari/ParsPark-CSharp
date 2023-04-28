using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ToolsLib
{
	public class MySerialize
	{
		public static byte[] Serialize(object anySerializableObject)
		{
			MemoryStream ms = new MemoryStream();
			BinaryFormatter bf1 = new BinaryFormatter();
			bf1.Serialize(ms, anySerializableObject);
			var userDataBytes = ms.ToArray();

			return userDataBytes;
		}

		public static object Deserialize(byte[] message)
		{
			MemoryStream ms = new MemoryStream(message);
			BinaryFormatter bf1 = new BinaryFormatter();
			ms.Position = 0;
			object rawObj = bf1.Deserialize(ms);

			return rawObj;
		}

		public static string SerializeObject(object o)
		{
			if (!o.GetType().IsSerializable)
			{
				return null;
			}

			using (MemoryStream stream = new MemoryStream())
			{
				new BinaryFormatter().Serialize(stream, o);
				return Convert.ToBase64String(stream.ToArray());
			}
		}

		public static object DeserializeObject(string str)
		{
			byte[] bytes = Convert.FromBase64String(str);

			using (MemoryStream stream = new MemoryStream(bytes))
			{
				return new BinaryFormatter().Deserialize(stream);
			}
		}

		// Convert an object to a byte array
		public static byte[] ObjectToByteArray(object obj)
		{
			if (obj == null)
				return null;

			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			bf.Serialize(ms, obj);

			return ms.ToArray();
		}

		// Convert a byte array to an Object
		public static object ByteArrayToObject(byte[] arrBytes)
		{
			MemoryStream memStream = new MemoryStream();
			BinaryFormatter binForm = new BinaryFormatter();
			memStream.Write(arrBytes, 0, arrBytes.Length);
			memStream.Seek(0, SeekOrigin.Begin);
			object obj = binForm.Deserialize(memStream);

			return obj;
		}
	}
}
