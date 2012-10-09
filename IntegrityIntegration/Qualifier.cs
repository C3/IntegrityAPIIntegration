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


