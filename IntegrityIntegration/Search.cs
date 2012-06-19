using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IntegrityIntegration
{
    class Search
    {
        IntegrityDataset _dataset;
        List<string> _conditions = new List<string>();
        public Search(IntegrityDataset dataset)
        {
            _dataset = dataset;
        }

        public List<string> Conditions
        {
            get { return _conditions; }
        }

        public string[] Perform(){
            string[] results = new string[0];
            return results;
        }


        internal void AddCondition(string columnName, string value)
        {
            _conditions.Add(string.Format("[{0}][{1}][exactly][]={2}", _dataset.m_tableName, columnName, value));
        }

        public string ToQueryConditions()
        {
            StringBuilder query = new StringBuilder();

            for (int i = 0; i < _dataset.m_qualifiers.Count; ++i)
            {
                query.Append(_dataset.m_qualifiers[i].ToSearchQuery(i));
                query.Append("&");
            }

            foreach (string condition in Conditions)
            {
                query.Append(condition);
                query.Append("&");
            }

            return query.ToString();
        }
    }
}
