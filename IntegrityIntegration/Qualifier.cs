
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using System.Linq;
using System.Text;

class Qualifier
{

	public Qualifier(string attributeName__1, int[] values__2)
	{
		AttributeName = attributeName__1;
		Values = values__2;
	}


	public int[] Values {
		get { return m_Values; }
		set { m_Values = value; }
	}
	private int[] m_Values;
	public string AttributeName {
		get { return m_AttributeName; }
		set { m_AttributeName = value; }
	}
	private string m_AttributeName;
}


