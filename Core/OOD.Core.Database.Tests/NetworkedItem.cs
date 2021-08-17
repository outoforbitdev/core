﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace OOD.Core.Database.Tests
{
    public class NetworkedItem
    {
        private string _defaultString = "";
        private string _valueString = "<?xml version=\"1.0\" encoding=\"utf-16\"?><ItemValue>two</ItemValue>";
        private NetworkedItem<EntityObject> Default()
        {
            return new NetworkedItem<EntityObject>("ItemValue", LocalTable.EntityObjectTable(), null);
        }
        [Fact]
        public void GetDefault()
        {
            Assert.Null(Default().Value);
        }
        [Fact]
        public void GetValue()
        {
            var value = Default();
            value.Value = LocalTable.EntityObjectTable()["two"];
            Assert.Equal(LocalTable.EntityObjectTable()["two"], value.Value);
        }
        [Fact]
        public void GetDefaultBool()
        {
            Assert.True(Default().UsingDefaultValue);
        }
        [Fact]
        public void GetValueBool()
        {
            var value = Default();
            value.Value = LocalTable.EntityObjectTable()["two"];
            Assert.False(value.UsingDefaultValue);
        }
        [Fact]
        public void SerializeDefault()
        {
            StringWriter stream = new StringWriter();
            XmlWriter writer = XmlWriter.Create(stream);
            Default().WriteXml(writer, true);
            writer.Flush();
            Assert.Equal(_defaultString, stream.ToString());
        }
        [Fact]
        public void DeserializeDefault()
        {
            StringReader stream = new StringReader(_defaultString);
            XmlReader reader = XmlReader.Create(stream);
            var value = Default();
            value.ReadXml(reader, true);
            Assert.Null(value.Value);
        }
        [Fact]
        public void SerializeValue()
        {
            var value = Default();
            value.Value = LocalTable.EntityObjectTable()["two"];
            StringWriter stream = new StringWriter();
            XmlWriter writer = XmlWriter.Create(stream);
            value.WriteXml(writer, true);
            writer.Flush();
            Assert.Equal(_valueString, stream.ToString());
        }
        [Fact]
        public void DeserializeValue()
        {
            NetworkedItem<EntityObject> value = Default();
            StringReader stream = new StringReader(_valueString);
            XmlReader reader = XmlReader.Create(stream);
            value.ReadXml(reader, true);
            Assert.Equal(LocalTable.EntityObjectTable()["two"], value.Value);
        }
    }
}
