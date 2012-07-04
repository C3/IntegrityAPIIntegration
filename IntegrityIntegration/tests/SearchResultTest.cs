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
            doc.CreateElement("people");
            XmlElement row = doc.CreateElement("row");

            //row.AppendChild(row. InnerText = "Tommy");
            //doc.AppendChild(row);

            //Record[] records = new Record[] { new Record(new XmlNode()) };
            Record[] records = new Record[0];
            Assert.AreEqual(records, result.Records()[0]);
        }
    }
}
