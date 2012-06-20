﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IntegrityIntegration
{
    class Search
    {
        const string PER_PAGE = "500";
        IntegrityDataset _dataset;
        List<string> _conditions = new List<string>();
        public Search(IntegrityDataset dataset)
        {
            AuditId = 0;
            _dataset = dataset;
        }

        internal List<string> Conditions
        {
            get { return _conditions; }
        }

        public int AuditId
        {
            get;
            set;
        }

        public string[] Perform(){
            string[] results = new string[0];
            return results;
        }

        internal void AddCondition(string columnName, string value)
        {
            _conditions.Add(string.Format("[{0}][{1}][exactly][]={2}", _dataset.m_tableName, columnName, value));
        }

        internal string PaginationParams(int pageNumber)
        {
            return string.Format("page={0}&per_page={1}", pageNumber, PER_PAGE);
        }

        internal string AuditIdParam()
        {
            return string.Format("audit_id_gt={0}", AuditId);
        }

        internal string ToQueryConditions()
        {
            List<string> queryParts = new List<string>();

            for (int i = 0; i < _dataset.m_qualifiers.Count; ++i)
            {
                queryParts.Add(_dataset.m_qualifiers[i].ToSearchQuery(i));
            }

            foreach (string condition in Conditions)
            {
                queryParts.Add(condition);
            }

            queryParts.Add(AuditIdParam());

            return string.Join("&", queryParts.ToArray());
        }
    }
}
