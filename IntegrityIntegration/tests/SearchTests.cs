using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using IntegrityIntegration;

namespace IntegrityAPITests
{

    [TestFixture()]
    public class SearchTests
    {

        [Test()]
        public void SearchWithNoResults(){
            IntegrityDataset ds = new IntegrityDataset();
            Search search = new Search(ds);
            string[] results = new string[0];
            Assert.AreEqual(search.Perform(), results);
        }

        [Test()]
        public void SearchWithResults()
        {
            IntegrityDataset ds = new IntegrityDataset();
            Search search = new Search(ds);
            string[] results = {"some result"};

            Assert.AreEqual(search.Perform(), results);
        }

        [Test()]
        public void AddCondition()
        {
            IntegrityDataset ds = new IntegrityDataset();
            ds.m_tableName = "people";
            Search search = new Search(ds);
            string condition = "[people][name][exactly][]=bob";
            string columnName, value;
            columnName = "name";
            value = "bob";

            List<string> conditions = new List<string> { condition };
            search.AddCondition(columnName, value);
            Assert.AreEqual(search.Conditions, conditions);
        }

    }


}
