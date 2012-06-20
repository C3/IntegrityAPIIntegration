﻿using System;
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
        string _audit_id_param;
        [SetUp()]
        public void TestSetup()
        {
            _audit_id_param = "audit_id_gt=0";
        }

        [Test()]
        public void SearchWithNoResults(){
            IntegrityDataset ds = new IntegrityDataset();
            Search search = new Search(ds);
            string[] results = new string[0];
            Assert.AreEqual(results, search.Execute());
        }

        [Test()]
        public void SearchWithResults()
        {
            IntegrityDataset ds = new IntegrityDataset();
            Search search = new Search(ds);
            string[] results = {"some result"};

            Assert.AreEqual(results, search.Execute());
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
            Assert.AreEqual(conditions, search.Conditions);
        }

        [Test()]
        public void SearchWithSingleCondition()
        {
            IntegrityDataset ds = new IntegrityDataset();
            ds.m_tableName = "people";
            Search search = new Search(ds);
            search.AddCondition("name", "bob");

            string query = "[people][name][exactly][]=bob&";
            query += _audit_id_param;
            Assert.AreEqual(query, search.ToQueryConditions());
        }

        [Test()]
        public void SearchWithMultipleConditions()
        {
            IntegrityDataset ds = new IntegrityDataset();
            ds.m_tableName = "people";
            Search search = new Search(ds);
            search.AddCondition("name", "bob");
            search.AddCondition("name", "sally");

            string query = "[people][name][exactly][]=bob&";
            query += "[people][name][exactly][]=sally&";
            query += _audit_id_param;
            Assert.AreEqual(query, search.ToQueryConditions());
        }

        [Test()]
        public void SearchWithSingleQualifier()
        {
            IntegrityDataset ds = new IntegrityDataset();
            ds.m_tableName = "people";
            List<Qualifier> qualifiers = new List<Qualifier> { new Qualifier("name", new string[] { "bob", "sally" }) };
            ds.m_qualifiers = qualifiers;
            Search search = new Search(ds);

            string query = "upload_qualifiers[0][dataset_attribute_name]=name";
            query += "&upload_qualifiers[0][qualifier_values][]=bob";
            query += "&upload_qualifiers[0][qualifier_values][]=sally&";
            query += _audit_id_param;

            Assert.AreEqual(query, search.ToQueryConditions());
        }

        [Test()]
        public void SearchWithMultipleQualifiers()
        {
            IntegrityDataset ds = new IntegrityDataset();
            ds.m_tableName = "people";
            List<Qualifier> qualifiers = new List<Qualifier>();
            qualifiers.Add(new Qualifier("name", new string[] { "bob" }));
            qualifiers.Add(new Qualifier("state", new string[] { "VIC" }));

            ds.m_qualifiers = qualifiers;
            Search search = new Search(ds);

            string query = "upload_qualifiers[0][dataset_attribute_name]=name";
            query += "&upload_qualifiers[0][qualifier_values][]=bob";
            query += "&upload_qualifiers[1][dataset_attribute_name]=state";
            query += "&upload_qualifiers[1][qualifier_values][]=VIC&";
            query += _audit_id_param;

            Assert.AreEqual(query, search.ToQueryConditions());
        }

        [Test()]
        public void SearchWithAuditIdParam()
        {
            IntegrityDataset ds = new IntegrityDataset();
            Search search = new Search(ds);
            search.AuditId = 100;

            string query = "audit_id_gt=100";

            Assert.AreEqual(query, search.ToQueryConditions());
        }

        [Test()]
        public void PaginationParameters()
        {
            IntegrityDataset ds = new IntegrityDataset();
            Search search = new Search(ds);

            Assert.AreEqual("page=1&per_page=500", search.PaginationParams(1));
        }

    }


}
