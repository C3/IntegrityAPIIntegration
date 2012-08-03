using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace IntegrityIntegration
{
    class SearchResult
    {
        private Record[] _records;
        private string searchResultXml;
        public SearchResult(string searchResultXml)
        {
            this.searchResultXml = searchResultXml;
        }

        public Record[] Records()
        {
            if (_records != null) { return _records; }

            List<Record> records = new List<Record>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(searchResultXml);

            foreach (XmlNode record in doc.SelectNodes("//row"))
            {
                records.Add(new Record(record));
            }
            _records = records.ToArray();
            return _records;
        }

    }
}
