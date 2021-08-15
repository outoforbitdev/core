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
    public class Entity
    {
        private string _defaultXmlString = 
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
            "<EntityObject id=\"thisisanidbruh\" />";
        private string _valueXmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
            "<EntityObject id=\"thisisanidbruh\">" +
                "<StringValue>a new string</StringValue>" +
                "<IntValue>84</IntValue>" +
                "<BoolValue>false</BoolValue>" +
            "</EntityObject>";
        private EntityObject Default()
        {
            return new EntityObject("thisisanidbruh");
        }
        private void AssertEntityValues(EntityObject entity, string stringValue, int intValue, bool boolValue)
        {
            Assert.Equal(stringValue, entity.StringValue.Value);
            Assert.Equal(intValue, entity.IntValue.Value);
            if (boolValue)
            {
                Assert.True(entity.BoolValue.Value);
            }
            else
            {
                Assert.False(entity.BoolValue.Value);
            }
        }
        [Fact]
        public void GetDefault()
        {
            AssertEntityValues(Default(), "default string value", 32, true);
        }
        [Fact]
        public void GetStringValue()
        {
            EntityObject entity = Default();
            entity.StringValue.Value = "a new string";
            AssertEntityValues(entity, "a new string", 32, true);
        }
        [Fact]
        public void GetIntValue()
        {
            EntityObject entity = Default();
            entity.IntValue.Value = 84;
            AssertEntityValues(entity, "default string value", 84, true);
        }
        [Fact]
        public void GetBoolValue()
        {
            EntityObject entity = Default();
            entity.BoolValue.Value = false;
            AssertEntityValues(entity, "default string value", 32, false);
        }
        [Fact]
        public void SerializeDefault()
        {
            StringWriter stream = new StringWriter();
            Default().SerializeToXml(stream);
            Assert.Equal(_defaultXmlString, stream.ToString());
        }
        [Fact]
        public void DeserializeDefault()
        {
            StringReader stream = new StringReader(_defaultXmlString);
            EntityObject value = Default();
            value.DeserializeFromXml(stream);
            Assert.Equal(Default(), value);
        }
        [Fact]
        public void SerializeValue()
        {
            StringWriter stream = new StringWriter();
            EntityObject value = Default();
            value.StringValue.Value = "a new string";
            value.IntValue.Value = 84;
            value.BoolValue.Value = false;
            value.SerializeToXml(stream);
            Assert.Equal(_valueXmlString, stream.ToString());
        }
        [Fact]
        public void DeserializeValue()
        {
            StringReader stream = new StringReader(_valueXmlString);
            EntityObject expected = Default();
            expected.StringValue.Value = "a new string";
            expected.IntValue.Value = 84;
            expected.BoolValue.Value = false;
            EntityObject value = Default();
            value.DeserializeFromXml(stream);
            Assert.Equal(expected, value);
        }
    }
}
