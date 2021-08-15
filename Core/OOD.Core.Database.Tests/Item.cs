using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace OOD.Core.Database.Tests
{
    public class Item
    {
        private string _defaultString = "";
        private string _valueString = "<?xml version=\"1.0\" encoding=\"utf-16\"?><ItemValue>real value</ItemValue>";
        private Item<string> Default()
        {
            return new Item<string>("ItemValue", "default");
        }
        [Fact]
        public void GetDefault()
        {
            Assert.Equal("default", Default().Value);
        }
        [Fact]
        public void GetValue()
        {
            var value = Default();
            value.Value = "real value";
            Assert.Equal("real value", value.Value);
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
            value.Value = "real value";
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
            Assert.Equal("default", value.Value);
        }
        [Fact]
        public void SerializeValue()
        {
            var value = Default();
            value.Value = "real value";
            StringWriter stream = new StringWriter();
            XmlWriter writer = XmlWriter.Create(stream);
            value.WriteXml(writer, true);
            writer.Flush();
            Assert.Equal(_valueString, stream.ToString());
        }
        [Fact]
        public void DeserializeValue()
        {
            var value = Default();
            value.Value = "real value";
            StringReader stream = new StringReader(_valueString);
            XmlReader reader = XmlReader.Create(stream);
            value.ReadXml(reader, true);
            Assert.Equal("real value", value.Value);
        }
    }
}
