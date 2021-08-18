using System;
using System.IO;

namespace OOD.Core.Serializable
{
    public interface ICSVSerializable
    {
        /// <summary>
        /// Converts an object to its CSV representation.
        /// </summary>
        /// <param name="stream">The TextWriter stream to which the object is serialized.</param>
        public void SerializeToCSV(TextWriter stream);

        /// <summary>
        /// Converts an object to its CSV representation.
        /// </summary>
        /// <returns>A string containing the CSV representation of the object.</returns>
        public string SerializeToCSV();

        /// <summary>
        /// Generates an object from its CSV representation.
        /// </summary>
        /// <param name="stream">The TextReader stream from which the object is deserialized.</param>
        public void DeserializeFromCSV(TextReader stream);

        /// <summary>
        /// Generates an object from its CSV representation.
        /// </summary>
        /// <param name="stream">The string from which the object is deserialized.</param>
        public void DeserializeFromCSV(string text);
    }
}
