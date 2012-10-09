/*
 * Copyright (C) 2012 C3 Products

 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to
 * whom the Software is furnished to do so,
 * subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IntegrityIntegration
{
    public class Search
    {
        const string PER_PAGE = "500";
        IntegrityDataset _dataset;
        List<string> _conditions = new List<string>();
        IIntegrityHttpService _service;

        public Search(IntegrityDataset dataset, IIntegrityHttpService service)
        {
            AuditId = 0;
            _dataset = dataset;
            _service = service;
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

        public Record[] Execute(){
            int pageNumber = 1;
            List<Record> allResults = new List<Record>();
            Record[] records;
            string resultXML;

            do {
                resultXML = _service.GetSearchResults(_dataset.m_id, ToQueryConditions() + "&" + PaginationParams(pageNumber));
                records = new SearchResult(resultXML).Records();
                allResults.AddRange(records);
                pageNumber += 1;
            } while(records.Length > 0);

            return allResults.ToArray();
        }

        public void AddCondition(string columnName, IEnumerable<string> values)
        {
          values.ToList().ForEach(val => AddCondition(columnName, val));
        }
        public void AddCondition(string columnName, string value)
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
