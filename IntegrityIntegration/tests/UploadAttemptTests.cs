
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;

namespace IntegrityAPITests
{
	public class UploadAttemptTests
	{

		[Test()]
		public void UploadAttempXmlTest()
		{
			string xml = "<user-access-group-authorisation>" + Environment.NewLine + " <name>Beyond Sync</name>" + Environment.NewLine + "  <datasets type=\"array\">" + Environment.NewLine + "    <dataset>" + Environment.NewLine + "      <id>45</id>" + Environment.NewLine + "      <name>Accreditation Type</name>" + Environment.NewLine + "      <is-bulk-allowed>false</is-bulk-allowed>" + Environment.NewLine + Environment.NewLine + "      <is-incremental-allowed>true</is-incremental-allowed>" + Environment.NewLine + "      <table-name>ct_accreditation_type</table-name>" + Environment.NewLine + "      <qualifiers type=\"array\">" + Environment.NewLine + "        <qualifier>" + Environment.NewLine + "          <dataset-attribute-name>RTP</dataset-attribute-name>" + Environment.NewLine + "          <valid-values type=\"array\">" + Environment.NewLine + "            <valid-value>83</valid-value>" + Environment.NewLine + "            <valid-value>90</valid-value>" + Environment.NewLine + "          </valid-values>" + Environment.NewLine + "        </qualifier>" + Environment.NewLine + "      </qualifiers>" + Environment.NewLine + "      <dataset-formats type=\"array\">" + Environment.NewLine + "        <dataset-format>" + Environment.NewLine + "          <name>XML</name>" + Environment.NewLine + "          <parser-type>XML</parser-type>" + Environment.NewLine + "        </dataset-format>" + Environment.NewLine + "      </dataset-formats>" + Environment.NewLine + "    </dataset>" + Environment.NewLine + "    <dataset>" + Environment.NewLine + "      <id>12</id>" + Environment.NewLine + "      <name>Accreditation Level</name>" + Environment.NewLine + Environment.NewLine + "      <is-bulk-allowed>false</is-bulk-allowed>" + Environment.NewLine + "      <is-incremental-allowed>false</is-incremental-allowed>" + Environment.NewLine + "      <table-name>ct_accreditation_level</table-name>" + Environment.NewLine + "      <qualifiers type=\"array\">" + Environment.NewLine + "      </qualifiers>" + Environment.NewLine + "      <dataset-formats type=\"array\">" + Environment.NewLine + "        <dataset-format>" + Environment.NewLine + Environment.NewLine + "          <name>CSV</name>" + Environment.NewLine + "          <parser-type>CSV</parser-type>" + Environment.NewLine + "        </dataset-format>" + Environment.NewLine + "      </dataset-formats>" + Environment.NewLine + "    </dataset>" + Environment.NewLine + "    <dataset>" + Environment.NewLine + "      <id>11</id>" + Environment.NewLine + "      <name>Accreditation</name>" + Environment.NewLine + Environment.NewLine + "      <is-bulk-allowed>false</is-bulk-allowed>" + Environment.NewLine + "      <is-incremental-allowed>false</is-incremental-allowed>" + Environment.NewLine + "      <table-name>ct_accreditation_level</table-name>" + Environment.NewLine + "      <qualifiers type=\"array\">" + Environment.NewLine + "      </qualifiers>" + Environment.NewLine + "      <dataset-formats type=\"array\">" + Environment.NewLine + "        <dataset-format>" + Environment.NewLine + Environment.NewLine + "          <name>XML</name>" + Environment.NewLine + "          <parser-type>XML</parser-type>" + Environment.NewLine + "        </dataset-format>" + Environment.NewLine + "      </dataset-formats>" + Environment.NewLine + "    </dataset>" + Environment.NewLine + "    <dataset>" + Environment.NewLine + "      <id>31</id>" + Environment.NewLine + "      <name>DS 01</name>" + Environment.NewLine + Environment.NewLine + "      <is-bulk-allowed>false</is-bulk-allowed>" + Environment.NewLine + "      <is-incremental-allowed>true</is-incremental-allowed>" + Environment.NewLine + "      <table-name>ct_accreditation_level</table-name>" + Environment.NewLine + "      <qualifiers type=\"array\">" + Environment.NewLine + "        <qualifier>" + Environment.NewLine + "          <dataset-attribute-name>Qual</dataset-attribute-name>" + Environment.NewLine + "          <valid-values type=\"array\">" + Environment.NewLine + "            <valid-value>11</valid-value>" + Environment.NewLine + "            <valid-value>54</valid-value>" + Environment.NewLine + "          </valid-values>" + Environment.NewLine + "        </qualifier>" + Environment.NewLine + "        <qualifier>" + Environment.NewLine + "          <dataset-attribute-name>Qual2</dataset-attribute-name>" + Environment.NewLine + "          <valid-values type=\"array\">" + Environment.NewLine + "            <valid-value>1</valid-value>" + Environment.NewLine + "            <valid-value>5</valid-value>" + Environment.NewLine + "          </valid-values>" + Environment.NewLine + "        </qualifier>" + Environment.NewLine + "      </qualifiers>" + Environment.NewLine + "      <dataset-formats type=\"array\">" + Environment.NewLine + "        <dataset-format>" + Environment.NewLine + Environment.NewLine + "          <name>XML</name>" + Environment.NewLine + "          <parser-type>XML</parser-type>" + Environment.NewLine + "        </dataset-format>" + Environment.NewLine + "      </dataset-formats>" + Environment.NewLine + "    </dataset>" + Environment.NewLine + "  </datasets>" + Environment.NewLine + "</user-access-group-authorisation>";
			Configuration configuration = new Configuration();
			configuration.BuildFromXml(xml);

			string fileXml = null;
			fileXml = "<rows><row><id>12</id><control_code>110</control_code></row></rows>";
			UploadAttempt _uploadAttempt = new UploadAttempt(configuration.GetDataset(45), configuration.GetQualifiersForDataset(45), ref fileXml, "XML");

			string resultXml = null;
			resultXml = _uploadAttempt.BuildAttemptXml();
			StringAssert.AreEqualIgnoringCase("RTP", ((Qualifier)_uploadAttempt.Qualifiers.ToArray().GetValue(0)).AttributeName);

			string expectedXML = null;
			expectedXML = "<upload-attempt><dataset-name>Accreditation Type</dataset-name><format-name>XML</format-name><bulk-or-incremental>incremental</bulk-or-incremental><qualifiers type=\"array\"><qualifier><dataset-attribute-name>RTP</dataset-attribute-name><qualifier-values type=\"array\"><qualifier-value>83</qualifier-value><qualifier-value>90</qualifier-value></qualifier-values></qualifier></qualifiers><file><![CDATA[<rows><row><id>12</id><control_code>110</control_code></row></rows>]]></file></upload-attempt>";

			StringAssert.AreEqualIgnoringCase(expectedXML, resultXml);
		}

	}
}
