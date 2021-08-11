using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using OOD.Core.Serializable;

namespace OOD.Core.Serializable.Tests
{
    class Implementor : FullSerializable, IEquatable<Implementor>
    {
        public int IntegerValue;
        public string StringValue;
        public bool BoolValue;

        public Implementor()
        {

        }
        public Implementor(int integerValue, string stringValue, bool boolValue)
        {
            IntegerValue = integerValue;
            StringValue = stringValue;
            BoolValue = boolValue;
        }

        public bool Equals(Implementor i)
        {
            return
                IntegerValue == i.IntegerValue &&
                BoolValue == i.BoolValue &&
                StringValue == i.StringValue;
        }

        public override string SerializeToCSV()
        {
            return
                IntegerValue.ToString() + "," +
                CSVSerializer.SerializeString(StringValue) + "," +
                BoolValue.ToString();
        }
        public override void DeserializeFromCSV(string text)
        {
            string[] fields = CSVSerializer.GetFields(text);
            IntegerValue = CSVSerializer.DeserializeInt(fields[0]);
            StringValue = CSVSerializer.DeserializeString(fields[1]);
            BoolValue = CSVSerializer.DeserializeBool(fields[2]);
        }
        public override void SerializeToXML(TextWriter stream)
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromXML(TextReader stream)
        {
            throw new NotImplementedException();
        }
        public override void SerializeToJSON(TextWriter stream)
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromJSON(TextReader stream)
        {
            throw new NotImplementedException();
        }
    }
}
