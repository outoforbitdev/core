using OOD.Core.Serializable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace OOD.Core.Database
{
    public abstract class Entity: XmlSerializable
    {
        public string ID
        {
            get { return _id; }
            set 
            { 
                if (ID == null)
                {
                    _id = value;
                } 
            }
        }
        protected string _id;
        protected string _tag = "Entity";
    }
}
