
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
        public void UploadAttemptIncrementalTest()
        {
            string xml = @"<user-access-group-authorisation><name>beyond sync</name><datasets type=""array""><dataset><id>45</id><name>accreditation type</name><is-bulk-allowed>false</is-bulk-allowed><is-incremental-allowed>true</is-incremental-allowed><table-name>ct_accreditation_type</table-name><qualifiers type=""array""><qualifier><dataset-attribute-name>rtp</dataset-attribute-name><valid-values type=""array""><valid-value>83</valid-value><valid-value>90</valid-value></valid-values></qualifier></qualifiers><dataset-formats type=""array""><dataset-format><name>xml</name><parser-type>xml</parser-type></dataset-format></dataset-formats></dataset><dataset><id>12</id><name>accreditation level</name><is-bulk-allowed>false</is-bulk-allowed><is-incremental-allowed>false</is-incremental-allowed><table-name>ct_accreditation_level</table-name><qualifiers type=""array""></qualifiers><dataset-formats type=""array""><dataset-format><name>csv</name><parser-type>csv</parser-type></dataset-format></dataset-formats></dataset><dataset><id>11</id><name>accreditation</name><is-bulk-allowed>false</is-bulk-allowed><is-incremental-allowed>false</is-incremental-allowed><table-name>ct_accreditation_level</table-name><qualifiers type=""array""></qualifiers><dataset-formats type=""array""><dataset-format><name>xml</name><parser-type>xml</parser-type></dataset-format></dataset-formats></dataset><dataset><id>31</id><name>ds 01</name><is-bulk-allowed>false</is-bulk-allowed><is-incremental-allowed>true</is-incremental-allowed><table-name>ct_accreditation_level</table-name><qualifiers type=""array""><qualifier><dataset-attribute-name>qual</dataset-attribute-name><valid-values type=""array""><valid-value>11</valid-value><valid-value>54</valid-value></valid-values></qualifier><qualifier><dataset-attribute-name>qual2</dataset-attribute-name><valid-values type=""array""><valid-value>1</valid-value><valid-value>5</valid-value></valid-values></qualifier></qualifiers><dataset-formats type=""array""><dataset-format><name>xml</name><parser-type>xml</parser-type></dataset-format></dataset-formats></dataset></datasets></user-access-group-authorisation>";
            Configuration configuration = new Configuration();
            configuration.BuildFromXml(xml);

            string fileXml = null;
            fileXml = "<rows><row><id>12</id><control_code>110</control_code></row></rows>";
            IntegrityDataset _dataset = configuration.GetDataset(45);
            DatasetFormat _format = _dataset.m_dataset_formats.Find(c => c.name == "xml");
            UploadAttempt _uploadAttempt = new UploadAttempt(_dataset, configuration.GetQualifiersForDataset(45), ref fileXml, _format, UploadAttempt.Type.Incremental);

            string resultXml = null;
            resultXml = _uploadAttempt.BuildAttemptXml();
            StringAssert.AreEqualIgnoringCase("RTP", ((Qualifier)_uploadAttempt.Qualifiers.ToArray().GetValue(0)).AttributeName);

            string expectedXML = null;
            expectedXML = "<upload-attempt><dataset-name>Accreditation Type</dataset-name><format-name>XML</format-name><bulk-or-incremental>incremental</bulk-or-incremental><qualifiers type=\"array\"><qualifier><dataset-attribute-name>RTP</dataset-attribute-name><qualifier-values type=\"array\"><qualifier-value>83</qualifier-value><qualifier-value>90</qualifier-value></qualifier-values></qualifier></qualifiers><file><![CDATA[<rows><row><id>12</id><control_code>110</control_code></row></rows>]]></file></upload-attempt>";

            StringAssert.AreEqualIgnoringCase(expectedXML, resultXml);
        }

        [Test()]
        public void UploadAttemptBulkTest()
        {
            string xml = @"<user-access-group-authorisation><name>beyond sync</name><datasets type=""array""><dataset><id>45</id><name>accreditation type</name><is-bulk-allowed>false</is-bulk-allowed><is-incremental-allowed>true</is-incremental-allowed><table-name>ct_accreditation_type</table-name><qualifiers type=""array""><qualifier><dataset-attribute-name>rtp</dataset-attribute-name><valid-values type=""array""><valid-value>83</valid-value><valid-value>90</valid-value></valid-values></qualifier></qualifiers><dataset-formats type=""array""><dataset-format><name>xml</name><parser-type>xml</parser-type></dataset-format></dataset-formats></dataset><dataset><id>12</id><name>accreditation level</name><is-bulk-allowed>false</is-bulk-allowed><is-incremental-allowed>false</is-incremental-allowed><table-name>ct_accreditation_level</table-name><qualifiers type=""array""></qualifiers><dataset-formats type=""array""><dataset-format><name>csv</name><parser-type>csv</parser-type></dataset-format></dataset-formats></dataset><dataset><id>11</id><name>accreditation</name><is-bulk-allowed>false</is-bulk-allowed><is-incremental-allowed>false</is-incremental-allowed><table-name>ct_accreditation_level</table-name><qualifiers type=""array""></qualifiers><dataset-formats type=""array""><dataset-format><name>xml</name><parser-type>xml</parser-type></dataset-format></dataset-formats></dataset><dataset><id>31</id><name>ds 01</name><is-bulk-allowed>false</is-bulk-allowed><is-incremental-allowed>true</is-incremental-allowed><table-name>ct_accreditation_level</table-name><qualifiers type=""array""><qualifier><dataset-attribute-name>qual</dataset-attribute-name><valid-values type=""array""><valid-value>11</valid-value><valid-value>54</valid-value></valid-values></qualifier><qualifier><dataset-attribute-name>qual2</dataset-attribute-name><valid-values type=""array""><valid-value>1</valid-value><valid-value>5</valid-value></valid-values></qualifier></qualifiers><dataset-formats type=""array""><dataset-format><name>xml</name><parser-type>xml</parser-type></dataset-format></dataset-formats></dataset></datasets></user-access-group-authorisation>";
            Configuration configuration = new Configuration();
            configuration.BuildFromXml(xml);

            string fileXml = null;
            fileXml = "<rows><row><id>12</id><control_code>110</control_code></row></rows>";
            IntegrityDataset _dataset = configuration.GetDataset(45);
            DatasetFormat _format = _dataset.m_dataset_formats.Find(c => c.name == "xml");
            UploadAttempt _uploadAttempt = new UploadAttempt(_dataset, configuration.GetQualifiersForDataset(45), ref fileXml, _format, UploadAttempt.Type.Bulk);

            string resultXml = null;
            resultXml = _uploadAttempt.BuildAttemptXml();
            StringAssert.AreEqualIgnoringCase("RTP", ((Qualifier)_uploadAttempt.Qualifiers.ToArray().GetValue(0)).AttributeName);

            string expectedXML = null;
            expectedXML = "<upload-attempt><dataset-name>Accreditation Type</dataset-name><format-name>XML</format-name><bulk-or-incremental>bulk</bulk-or-incremental><qualifiers type=\"array\"><qualifier><dataset-attribute-name>RTP</dataset-attribute-name><qualifier-values type=\"array\"><qualifier-value>83</qualifier-value><qualifier-value>90</qualifier-value></qualifier-values></qualifier></qualifiers><file><![CDATA[<rows><row><id>12</id><control_code>110</control_code></row></rows>]]></file></upload-attempt>";

            StringAssert.AreEqualIgnoringCase(expectedXML, resultXml);
        }

	}
}

