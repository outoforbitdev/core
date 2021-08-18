using System;
using System.IO;

namespace OOD.Core.Serializable
{
    public interface IJSONSerializable
    {
        /// <summary>
        /// Converts an object to its JSON representation.
        /// </summary>
        /// <param name="stream">The TextWriter stream to which the object is serialized.</param>
        public void SerializeToJSON(TextWriter stream);

        /// <summary>
        /// Generates an object from its JSON representation.
        /// </summary>
        /// <param name="stream">The TextReader stream from which the object is deserialized.</param>
        public void DeserializeFromJSON(TextReader stream);
    }
}
