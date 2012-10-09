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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml;

class Configuration
{


	public List<IntegrityDataset> m_Datasets;
	public List<Qualifier> GetQualifiersForDataset(int dataset_id)
	{
		return GetDataset(dataset_id).m_qualifiers;
	}

	public IntegrityDataset GetDataset(int dataset_id)
	{
		foreach (IntegrityDataset dataset in m_Datasets) {
			if (dataset.m_id == dataset_id) {
				return dataset;
			}
		}

        throw new Exception("No data set with id '" + dataset_id + "' found");
	}

    public IntegrityDataset GetDataset(string datasetName)
    {
        foreach (IntegrityDataset dataset in m_Datasets)
        {
            if (dataset.m_name.Equals(datasetName))
            {
                return dataset;
            }
        }
        throw new Exception("No data set named '" + datasetName + "' found");
    }

	public void BuildFromXml(string xml_config)
	{
		m_Datasets = new List<IntegrityDataset>();

		XmlDocument xmld = new XmlDocument();
		xmld.LoadXml(xml_config);

		XmlNode ds_node = null;

		foreach (XmlNode ds_node_loopVariable in xmld.SelectNodes("user-access-group-authorisation/datasets/dataset")) {
			ds_node = ds_node_loopVariable;
			IntegrityDataset ds = buildDataset(ds_node);
			m_Datasets.Add(ds);
		}

	}

	private IntegrityDataset buildDataset(XmlNode ds_node)
	{
		dynamic ds = new IntegrityDataset();
		ds.m_id = int.Parse(ds_node.SelectSingleNode("id").InnerText);
		ds.m_bulk_allowed = bool.Parse(ds_node.SelectSingleNode("is-bulk-allowed").InnerText);
		ds.m_incremental_allowed = bool.Parse(ds_node.SelectSingleNode("is-incremental-allowed").InnerText);
		ds.m_name = ds_node.SelectSingleNode("name").InnerText;
		ds.m_tableName = ds_node.SelectSingleNode("table-name").InnerText;
		ds.m_qualifiers = new List<Qualifier>();

		XmlNode q_node = null;
		foreach (XmlNode q_node_loopVariable in ds_node.SelectNodes("qualifiers/qualifier")) {
			q_node = q_node_loopVariable;
			ds.m_qualifiers.Add(buildQualifier(q_node));
		}

		return ds;
	}

	private Qualifier buildQualifier(XmlNode q_node)
	{
		string name = null;
		name = q_node.SelectSingleNode("dataset-attribute-name").InnerText;

		Qualifier qualifier = new Qualifier(name, getValidValues(q_node.SelectNodes("valid-values/valid-value")));
		return qualifier;
	}

	private string[] getValidValues(XmlNodeList val_nodes)
	{
		List<string> values = new List<string>();
		XmlNode val_node = null;
		foreach (XmlNode val_node_loopVariable in val_nodes) {
			val_node = val_node_loopVariable;
			values.Add(val_node.InnerText);
		}

		return values.ToArray();
	}
}
