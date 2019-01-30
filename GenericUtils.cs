using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public static class GenericUtils {

	public static bool Save<T>(string FileName, Object Obj) {
		var xs = new XmlSerializer(typeof(T));
		using(TextWriter sw = new StreamWriter(FileName)) {
			xs.Serialize(sw, Obj);
		}

		if(File.Exists(FileName))
			return true;
		else return false;
	}

	public static T Load<T>(string FileName) {
		Object rslt;

		if(File.Exists(FileName)) {
			var xs = new XmlSerializer(typeof(T));

			using(var sr = new StreamReader(FileName)) {
				rslt = (T)xs.Deserialize(sr);
			}
			return (T)rslt;
		} else {
			return default(T);
		}
	}

	/// <summary>
	/// Perform a deep Copy of the object.
	/// </summary>
	/// <typeparam name="T">The type of object being copied.</typeparam>
	/// <param name="source">The object instance to copy.</param>
	/// <returns>The copied object.</returns>
	public static T Clone<T>(T source) {
		if(!typeof(T).IsSerializable) {
			throw new ArgumentException("The type must be serializable.", "source");
		}

		// Don't serialize a null object, simply return the default for that object
		if(Object.ReferenceEquals(source, null)) {
			return default(T);
		}

		IFormatter formatter = new BinaryFormatter();
		Stream stream = new MemoryStream();
		using(stream) {
			formatter.Serialize(stream, source);
			stream.Seek(0, SeekOrigin.Begin);
			return (T)formatter.Deserialize(stream);
		}
	}
}