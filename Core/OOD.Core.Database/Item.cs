using OOD.Core.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OOD.Core.Database
{
    public class Item<T> : XmlSerializable, IEquatable<Item<T>>
        where T: IEquatable<T>
    {
        protected T _defaultValue;
        protected T _value;
        protected new readonly string _tag;
        protected static readonly System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        public T Value
        {
            get
            {
                if (_useDefaultValue)
                {
                    return _defaultValue;
                }
                return _value;
            }
            set { 
                _value = value;
                _useDefaultValue = false;
            }   
        }
        public bool UsingDefaultValue { get { return _useDefaultValue; } }
        protected bool _useDefaultValue;
        public Item(string tag, T defaultValue)
        {
            _tag = tag;
            _defaultValue = defaultValue;
            _useDefaultValue = true;
        }
        public override void ReadXml(XmlReader reader, bool ignoreItemTag = false)
        {
            try
            {
                _useDefaultValue = !DeserializeProperty(reader, serializer, _tag, out _value, true);
            }
            finally
            {
                
            }
        }

        public override void WriteXml(XmlWriter writer, bool ignoreItemTag = false)
        {
            if (!UsingDefaultValue)
            {
                SerializeProperty(writer, serializer, _tag, _value, true);
            }
        }

        public bool Equals(Item<T> other)
        {
            return UsingDefaultValue == other.UsingDefaultValue && Value.Equals(other.Value);
        }

        public static bool operator ==(Item<T> a, Item<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Item<T> a, Item<T> b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            if (o is Item<T>)
            {
                return ((Item<T>)o).Equals(this);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return
                _defaultValue.GetHashCode() +
                _value.GetHashCode() +
                _tag.GetHashCode();
        }
    }
}
