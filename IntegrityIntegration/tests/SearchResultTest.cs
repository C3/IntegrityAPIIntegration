using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using IntegrityIntegration;
using System.Xml;

namespace IntegrityAPITests
{
    [TestFixture()]
    public class SearchResultTest
    {

        [Test()]
        public void SearchResultRecords()
        {
            SearchResult result = new SearchResult("<people><row><name>Tommy</name></row></people>");
            XmlDocument doc = new XmlDocument();

            XmlElement row = doc.CreateElement("row");
            XmlElement name = doc.CreateElement("name");
            name.InnerText = "Tommy";

            row.AppendChild(name);
            doc.AppendChild(row);
            Record[] records = new Record[] { new Record(doc.ChildNodes[0]) };

            Assert.AreEqual(records[0]["name"], result.Records()[0]["name"]);
        }

        [Test()]
        public void SearchReultMultipleRecords()
        {
            SearchResult result = new SearchResult("<people><row><name>Tommy</name></row><row><name>Billy</name></row></people>");

            Assert.Greater(result.Records().Length, 1);
        }

        [Test()]
        public void SearchResultWithNoRescords()
        {
            SearchResult result = new SearchResult("<people></people>");

            Record[] records = new Record[0];
            Assert.AreEqual(records, result.Records());
        }

    }
}
