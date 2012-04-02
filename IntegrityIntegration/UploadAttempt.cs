
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

			foreach (int value in qualifier.Values) {
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

