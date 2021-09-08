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
    using TestSetType = SerializableHashSet<SerializableObject>;
    public class SerializableHashSet
    {
        private TestSetType TestSet()
        {
            TestSetType set = new TestSetType();
            set.Add(new SerializableObject(1, true));
            set.Add(new SerializableObject(3, false));
            return set;
        }

        #region XML
        private const string _simpleString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
            "<SerializableDictionary>" +
                "<Item>" +
                    "<Key>first</Key>" +
                    "<Value>" +
                        "<Number>1</Number>" +
                        "<Boolean>true</Boolean>" +
                    "</Value>" +
                "</Item>" +
                "<Item>" +
                    "<Key>second</Key>" +
                    "<Value>" +
                        "<Number>3</Number>" +
                        "<Boolean>false</Boolean>" +
                    "</Value>" +
                "</Item>" +
            "</SerializableDictionary>";

        [Fact]
        public void SerializeSimple()
        {
            var stream = new StringWriter();

            TestSet().SerializeToXml(stream);
            Assert.Equal(_simpleString, stream.ToString());
        }
        [Fact]
        public void DeserializeSimple()
        {
            var stream = new StringReader(_simpleString);
            TestSetType result = new TestSetType();
            result.DeserializeFromXml(stream);
            Assert.Equal(TestSet(), result);
        }
        #endregion XML
    
        [Fact]
        public void AddDuplicate()
        {
            SerializableObject testObject = new SerializableObject(8, false);
            TestSetType set = TestSet();
            Assert.Contains(testObject, set);
        }
    }
}
