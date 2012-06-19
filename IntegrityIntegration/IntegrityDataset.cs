
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

class IntegrityDataset
{

	public int m_id;
	public string m_name;
	public string m_tableName;
	public bool m_bulk_allowed;

	public bool m_incremental_allowed;
	public List<Qualifier> m_qualifiers = new List<Qualifier>();
}
