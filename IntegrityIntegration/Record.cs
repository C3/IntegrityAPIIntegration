using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace IntegrityIntegration
{
    public class Record
    {
        Dictionary<string, string> columnValues;
        private XmlNode recordXml;
        public Record(XmlNode recordXml)
        {
            this.recordXml = recordXml;
            columnValues = new Dictionary<string, string>(recordXml.ChildNodes.Count);
            foreach (XmlNode column in recordXml.ChildNodes)
            {
                columnValues[column.Name] = column.InnerText;
            }

        }

        public IEnumerable<string> columns()
        {
          return this.columnValues.Keys;
        }
        public string this[string columnName]
        {
            get { return columnValues[columnName]; }
        }

    }
}
