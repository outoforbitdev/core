﻿using OOD.Core.Serializable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace OOD.Core.Database
{
    public abstract class Entity: XmlSerializable
    {
        public string ID
        {
            get { return _id; }
            set 
            { 
                if (ID == null)
                {
                    _id = value;
                } 
            }
        }
        protected string _id;
        protected new string _tag = "Entity";

        protected abstract void Clear();
        public new void DeserializeFromXml(TextReader stream)
        {
            Clear();
            XmlReader reader = XmlReader.Create(stream);
            ReadXml(reader, false);
            reader.Close();
        }

        protected static void DeserializeItemFromXml<T>(XmlReader reader, Item<T> item)
            where T: IEquatable<T>
        {
            item.ReadXml(reader, false);
        }

        public new void SerializeToXml(TextWriter stream)
        {
            XmlWriter writer = XmlWriter.Create(stream);
            WriteXml(writer, false);
            writer.Flush();
            writer.Close();
        }

        protected static void SerializeItemToXml<T>(XmlWriter writer, Item<T> item)
            where T : IEquatable<T>
        {
            item.WriteXml(writer, false);
        }
    }
}
