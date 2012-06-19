using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace IntegrityIntegration
{

    [TestFixture()]
    class QualifierTest
    {
        [Test()]
        public void ToSearchQueryWithOneValue(){
            Qualifier qualifier = new Qualifier("state", new string [] {"VIC"});
            string query = "upload_qualifiers[0][dataset_attribute_name]=state";
            query += "&upload_qualifiers[0][qualifier_values][]=VIC";

            Assert.AreEqual(query, qualifier.ToSearchQuery(0));
        }

        [Test()]
        public void ToSearchQueryWithMultipleValues(){
            Qualifier qualifier = new Qualifier("state name", new string[] { "VIC", "ACT & NSW" });
            string query = "upload_qualifiers[0][dataset_attribute_name]=state+name";
            query += "&upload_qualifiers[0][qualifier_values][]=VIC";
            query += "&upload_qualifiers[0][qualifier_values][]=ACT+%26+NSW";

            Assert.AreEqual(query, qualifier.ToSearchQuery(0));
        }
    }
}
