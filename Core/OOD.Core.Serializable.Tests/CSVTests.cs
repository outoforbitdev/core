using System;
using System.IO;
using Xunit;

namespace OOD.Core.Serializable.Tests
{
    public class CSVTests
    {
        private string _simple = "3,\"this is a string\",True";
        private Implementor i = new Implementor(3, "this is a string", true);
        [Fact]
        public void SerializeSimple()
        {
            var stream = new StringWriter();

            i.SerializeToCSV(stream);
            Assert.Equal(_simple, stream.ToString());
        }
        [Fact]
        public void DeserializeSimple()
        {
            var stream = new StringReader(_simple);

            Implementor k = new Implementor();
            k.DeserializeFromCSV(stream);
            Assert.Equal(i, k);
        }
        private string _serializedString = "\"include a \\\"quote\\\" in the string\"";
        private string _string = "include a \"quote\" in the string";
        [Fact]
        public void SerializeString()
        {
            string str = CSVSerializer.SerializeString(_string);
            Assert.Equal(_serializedString, str);
        }
        [Fact]
        public void DeserializeString()
        {
            string str = CSVSerializer.DeserializeString(_serializedString.Trim('"'));
            Assert.Equal(_string, str);
        }
    }
}
