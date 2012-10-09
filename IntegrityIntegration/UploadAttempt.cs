/*
 * Copyright (C) 2012 C3 Products

 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml;


class UploadAttempt
{

	private int _id;
	private IntegrityDataset _dataset;
	private List<Qualifier> _qualifiers;
	private string _formatName;

	private string _fileXml;
	
    public UploadAttempt(IntegrityDataset ds, List<Qualifier> qualifiers, ref string payload, string format_name)
	{
		this._dataset = ds;
		this._qualifiers = qualifiers;
		this._fileXml = payload;
		_formatName = format_name;
	}

	public string BuildAttemptXml()
	{

		XmlDocument uaXml = new XmlDocument();
		XmlElement element = null;
		XmlElement qualifiersElement = null;
		XmlElement qualifierElement = null;
		XmlElement valuesElement = null;
		XmlElement fileElement = null;
		XmlAttribute attribute = null;
		XmlElement docRoot = null;
		docRoot = uaXml.CreateElement("upload-attempt");
		uaXml.AppendChild(docRoot);

		element = uaXml.CreateElement("dataset-name");
		element.InnerText = _dataset.m_name;
		docRoot.AppendChild(element);

		element = uaXml.CreateElement("format-name");
		element.InnerText = _formatName;
		docRoot.AppendChild(element);

		element = uaXml.CreateElement("bulk-or-incremental");
		element.InnerText = "incremental";
		docRoot.AppendChild(element);

		qualifiersElement = uaXml.CreateElement("qualifiers");
		attribute = uaXml.CreateAttribute("type");
		attribute.Value = "array";
		qualifiersElement.Attributes.Append(attribute);

		foreach (Qualifier qualifier in Qualifiers) {
			qualifierElement = uaXml.CreateElement("qualifier");
			element = uaXml.CreateElement("dataset-attribute-name");
			element.InnerText = qualifier.AttributeName;
			qualifierElement.AppendChild(element);

			valuesElement = uaXml.CreateElement("qualifier-values");
			valuesElement.Attributes.Append((XmlAttribute)attribute.Clone());

			foreach (string value in qualifier.Values) {
				element = uaXml.CreateElement("qualifier-value");
				element.InnerText = value.ToString();
				valuesElement.AppendChild(element);
			}
			qualifierElement.AppendChild(valuesElement);
			qualifiersElement.AppendChild(qualifierElement);
		}
		docRoot.AppendChild(qualifiersElement);

		fileElement = uaXml.CreateElement("file");
		dynamic cData = uaXml.CreateCDataSection(_fileXml);
		fileElement.AppendChild(cData);

		docRoot.AppendChild(fileElement);

		return uaXml.InnerXml;
	}


	public int ID {
		get { return _id; }
	}

	public List<Qualifier> Qualifiers {
		get { return _qualifiers; }
	}

}

