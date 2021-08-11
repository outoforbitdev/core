using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD.Core.Serializable
{
    public class CSVUnparseableException: Exception
    {
        public CSVUnparseableException(string message) : base(message) { }
    }
    public static class CSVSerializer
    {
        public static string SerializeString(string text)
        {
            return "\"" + text.Replace("\"", "\\\"") + "\"";
        }
        public static string DeserializeString(string text)
        {
            return text.Replace("\\\"", "\"");
        }
        public static int DeserializeInt(string text)
        {
            int value;
            if (int.TryParse(text, out value))
            {
                return value;
            }
            throw new CSVUnparseableException("Could not parse as int: " + text);
        }
        public static bool DeserializeBool(string text)
        {
            bool value;
            if (bool.TryParse(text, out value))
            {
                return value;
            }
            throw new CSVUnparseableException("Could not parse as bool: " + text);
        }
        public static string[] GetFields(string text)
        {
            TextFieldParser parser = new TextFieldParser(new StringReader(text));
            parser.SetDelimiters(new string[] { "," });
            return parser.ReadFields();
        }
    }
}
