using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

namespace OOD.Core.Serializable.Tests
{
    public class XMLTests
    {
        private static readonly SerializableObject _simpleObject = new SerializableObject(1, true);
        private const string _simpleString = 
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
            "<SerializableObject>" +
            "<Number>1</Number>" +
            "<Boolean>true</Boolean>" +
            "</SerializableObject>";

        [Fact]
        public void SerializeSimple()
        {
            var stream = new StringWriter();

            _simpleObject.SerializeToXML(stream);
            Assert.Equal(_simpleString, stream.ToString());
        }
        [Fact]
        public void DeserializeSimple()
        {
            var stream = new StringReader(_simpleString);
            SerializableObject result = new SerializableObject(0, false);
            result.DeserializeFromXML(stream);
            Assert.Equal(_simpleObject, result);
        }
    }
}
