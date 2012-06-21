using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NUnit.Framework;

namespace IntegrityIntegration
{
    [TestFixture()]
    public class RecordTest
    {

        [Test()]
        public void RecordFieldAccess()
        {
            string resultXml = "<people><row><name>Tommy</name></row></people>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(resultXml);
            XmlNode tommy = doc.SelectSingleNode("//row");
            
            Assert.AreEqual("Tommy", new Record(tommy)["name"]);
        }
    }
}
