using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OOD.Core.Serializable.Tests
{
    public class SerializableObject : XmlSerializable, IEquatable<SerializableObject>
    {
        private const string _defaultTag = "SerializableObject";
        protected new string _tag
        {
            get { return _defaultTag; }
        }
        public int Number;
        private const string _defaultNumberTag = "Number";
        protected string _numberTag
        {
            get { return _defaultNumberTag; }
        }
        private bool _boolean;
        private const string _defaultBooleanTag = "Boolean";
        protected string _booleanTag
        {
            get { return _defaultBooleanTag; }
        }

        public SerializableObject()
        {
            Number = 0;
            _boolean = true;
        }
        public SerializableObject(int number, bool boolean)
        {
            Number = number;
            _boolean = boolean;
        }

        public override void ReadXml(XmlReader reader, bool includeStartEndElement)
        {
            SerializableObject.AttemptReadStartElement(reader, _tag, includeStartEndElement);
            DeserializeCLRProperty(reader, _numberTag, out Number);
            DeserializeCLRProperty(reader, _booleanTag, out _boolean);
            SerializableObject.AttemptReadEndElement(reader, includeStartEndElement);
        }

        public override void WriteXml(XmlWriter writer, bool includeStartEndElement)
        {
            SerializableObject.AttemptWriteStartElement(writer, _tag, includeStartEndElement);
            SerializeCLRProperty(writer, _numberTag, Number);
            SerializeCLRProperty(writer, _booleanTag, _boolean);
            SerializableObject.AttemptWriteEndElement(writer, includeStartEndElement);
        }

        public bool Equals(SerializableObject other)
        {
            return
                Number == other.Number &&
                _boolean == other._boolean;
        }
    }
}
