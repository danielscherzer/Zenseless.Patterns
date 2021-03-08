using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Zenseless.Patterns
{
	/// <summary>
	/// Contains class instance serialization/deserialization methods. 
	/// Can be used for persisting class instances to disc and reading them back to memory.
	/// </summary>
	public static class XmlSerializationExtensions
	{
		/// <summary>
		/// Deserializes from an XML file into a new class instance of a given type.
		/// </summary>
		/// <param name="fileName">The file name from which the serialized instance will be restored from.</param>
		/// <param name="type">The type of the class that will be deserialized.</param>
		/// <returns>Deserialized class instance</returns>
		public static DataType? FromXMLFile<DataType>(this string fileName)
		{
			using StreamReader inFile = new StreamReader(fileName);
			XmlSerializer formatter = new XmlSerializer(typeof(DataType));
			return (DataType)formatter.Deserialize(inFile);
		}

		/// <summary>
		/// Deserializes from an XML string into a new class instance of a given type.
		/// </summary>
		/// <param name="xmlString">XML string from which to deserialize.</param>
		/// <param name="type">The type of the class that will be deserialized.</param>
		/// <returns>Deserialized class instance</returns>
		public static DataType? FromXmlString<DataType>(this string xmlString)
		{
			using StringReader input = new StringReader(xmlString);
			XmlSerializer formatter = new XmlSerializer(typeof(DataType));
			return (DataType)formatter.Deserialize(input);
		}

		/// <summary>
		/// Serializes the given class instance into a XML format file.
		/// </summary>
		/// <param name="serializable">The class instance to be serialized.</param>
		/// <param name="fileName">The file name the serialized instance will be stored to.</param>
		public static void ToXMLFile(this object serializable, string fileName)
		{
			if (serializable is null) throw new ArgumentNullException(nameof(serializable));

			XmlSerializer formatter = new XmlSerializer(serializable.GetType());
			using StreamWriter outfile = new StreamWriter(fileName);
			formatter.Serialize(outfile, serializable);
		}

		/// <summary>
		/// Serializes the given class instance into a XML string.
		/// </summary>
		/// <param name="serializable">The class instance to be serialized.</param>
		public static string ToXmlString(this object serializable)
		{
			if (serializable is null) throw new ArgumentNullException(nameof(serializable));
			XmlSerializer formatter = new XmlSerializer(serializable.GetType());
			StringBuilder builder = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings
			{
				Encoding = Encoding.Default,
				Indent = false,
				OmitXmlDeclaration = true,
				NamespaceHandling = NamespaceHandling.OmitDuplicates
			};
			using (XmlWriter writer = XmlWriter.Create(builder, settings))
			{
				formatter.Serialize(writer, serializable);
			}
			string output = builder.ToString();
			return output;
		}
	}
}
