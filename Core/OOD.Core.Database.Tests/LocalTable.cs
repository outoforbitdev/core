using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OOD.Core.Database.Tests
{
    public class LocalTable
    {
        private string _string =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
            "<SerializableDictionary>" +
                "<Item>" +
                    "<Key>one</Key>" +
                    "<Value id=\"one\" />" +
                "</Item>" +
                "<Item>" +
                    "<Key>two</Key>" +
                    "<Value id=\"two\">" +
                        "<StringValue>two</StringValue>" +
                        "<IntValue>2</IntValue>" +
                        "<BoolValue>false</BoolValue>" +
                    "</Value>" +
                "</Item>" +
                "<Item>" +
                    "<Key>three</Key>" +
                    "<Value id=\"three\">" +
                        "<StringValue>a much longer string</StringValue>" +
                        "<IntValue>59665</IntValue>" +
                        "<BoolValue>true</BoolValue>" +
                    "</Value>" +
                "</Item>" +
            "</SerializableDictionary>";

        [Fact]
        public void NonSerializedAttribute()
        {
            EntityObject entityOne = new EntityObject("one");
            EntityObject entityTwo = new EntityObject("two", "two", 2, false);
            EntityObject entityThree = new EntityObject("three", "a much longer string", 59665, true);

            Database db = new Database();
            LocalTable<EntityObject> table = new LocalTable<EntityObject>(db);

            table.AddOrUpdateEntity(entityOne);
            table.AddOrUpdateEntity(entityTwo);
            table.AddOrUpdateEntity(entityThree);

            StringWriter stream = new StringWriter();
            table.SaveAs(stream);
            Assert.Equal(_string, stream.ToString());
        }
    }
}
