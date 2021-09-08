using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;
using OOD.Core.Serializable.Tests;

namespace OOD.Core.Collections.Tests
{
    using TestDicType = SerializableDictionary<string, SerializableObject>;
    public class SerializableDictionary
    {
        private TestDicType TestDictionary()
        {
            TestDicType dictionary = new TestDicType();
            dictionary.Add("first", new SerializableObject(1, true));
            dictionary.Add("second", new SerializableObject(3, false));
            return dictionary;
        }

        #region XML
        private const string _simpleString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
            "<SerializableDictionary>" +
                "<Item>" +
                    "<Key>first</Key>" +
                    "<SerializableObject>" +
                        "<Number>1</Number>" +
                        "<Boolean>true</Boolean>" +
                    "</SerializableObject>" +
                "</Item>" +
                "<Item>" +
                    "<Key>second</Key>" +
                    "<SerializableObject>" +
                        "<Number>3</Number>" +
                        "<Boolean>false</Boolean>" +
                    "</SerializableObject>" +
                "</Item>" +
            "</SerializableDictionary>";

        [Fact]
        public void SerializeSimple()
        {
            var stream = new StringWriter();

            TestDictionary().SerializeToXmlStream(stream);
            Assert.Equal(_simpleString, stream.ToString());
        }
        [Fact]
        public void DeserializeSimple()
        {
            var stream = new StringReader(_simpleString);
            TestDicType result = new TestDicType();
            result.DeserializeFromXmlStream(stream);
            Assert.Equal(TestDictionary(), result);
        }
        #endregion XML
    
        [Fact]
        public void AddDuplicate()
        {
            SerializableObject testObject = new SerializableObject(8, false);
            TestDicType dictionary = TestDictionary();
            dictionary["first"] = testObject;
            Assert.Equal(testObject, dictionary["first"]);
        }
    }
}
