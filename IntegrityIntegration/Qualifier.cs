
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using System.Linq;
using System.Text;
using System.Web;

public class Qualifier
{

	public Qualifier(string attributeName__1, string[] values__2)
	{
		AttributeName = attributeName__1;
		Values = values__2;
	}


	public string[] Values {
		get { return m_Values; }
		set { m_Values = value; }
	}
	private string[] m_Values;
	public string AttributeName {
		get { return m_AttributeName; }
		set { m_AttributeName = value; }
	}
	private string m_AttributeName;

    public string ToSearchQuery(int index)
    {
        string query = "";
        query += string.Format("upload_qualifiers[{0}][dataset_attribute_name]={1}", index, HttpUtility.UrlEncode(AttributeName));
        foreach(string value in Values){
            query += string.Format("&upload_qualifiers[{0}][qualifier_values][]={1}", index, HttpUtility.UrlEncode(value));
        }
        return query;
    }
}


