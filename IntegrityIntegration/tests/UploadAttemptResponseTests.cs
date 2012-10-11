
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;

using IntegrityAPI;

namespace IntegrityAPITests
{
	public class UploadAttemptResponseTests
	{

		[Test()]
		public void UploadAttempXmlValidatedTest()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine + "<upload-attempt>" + Environment.NewLine + "<id>4048</id>" + Environment.NewLine + "<status>upload</status>" + Environment.NewLine + "<rows-uploaded>99</rows-uploaded>" + Environment.NewLine + "<uploaded-row-data><tbl_reg type=\"array\"></tbl_reg></uploaded-row-data>" + Environment.NewLine + "<row-errors type=\"array\"></row-errors>" + Environment.NewLine + "<errors></errors>" + Environment.NewLine + "</upload-attempt>";

			UploadAttemptResponse response = new UploadAttemptResponse(xml);
			Assert.IsTrue(response.WasSuccess);
			Assert.IsFalse(response.IsPending);
			Assert.AreEqual(response.GetId, 4048);
		}

		[Test()]
		public void UploadAttempXmlUploadedTest()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine + "<upload-attempt>" + Environment.NewLine + "<id>4048</id>" + Environment.NewLine + "<status>pending</status>" + Environment.NewLine + "<rows-uploaded>99</rows-uploaded>" + Environment.NewLine + "<uploaded-row-data><tbl_reg type=\"array\"></tbl_reg></uploaded-row-data>" + Environment.NewLine + "<row-errors type=\"array\"></row-errors>" + Environment.NewLine + "<errors></errors>" + Environment.NewLine + "</upload-attempt>";

			UploadAttemptResponse response = new UploadAttemptResponse(xml);
			Assert.IsTrue(response.WasSuccess);
			Assert.IsTrue(response.IsPending);
			Assert.AreEqual(response.GetId, 4048);
		}

		[Test()]
		public void UploadAttempXmlCreatedTest()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine + "<upload-attempt>" + Environment.NewLine + "<id>4048</id>" + Environment.NewLine + "<status>validate</status>" + Environment.NewLine + "<rows-uploaded>99</rows-uploaded>" + Environment.NewLine + "<uploaded-row-data><tbl_reg type=\"array\"></tbl_reg></uploaded-row-data>" + Environment.NewLine + "<row-errors type=\"array\"></row-errors>" + Environment.NewLine + "<errors></errors>" + Environment.NewLine + "</upload-attempt>";

			UploadAttemptResponse response = new UploadAttemptResponse(xml);
			Assert.IsTrue(response.WasSuccess);
			Assert.IsFalse(response.IsPending);
			Assert.AreEqual(response.GetId, 4048);
		}

		[Test()]
		public void UploadAttempXmlCreatFailureTest()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine + "<upload-attempt>" + Environment.NewLine + "<id></id>" + Environment.NewLine + "<status></status>" + Environment.NewLine + "<rows-uploaded>99</rows-uploaded>" + Environment.NewLine + "<uploaded-row-data><tbl_reg type=\"array\"></tbl_reg></uploaded-row-data>" + Environment.NewLine + "<row-errors type=\"array\"></row-errors>" + Environment.NewLine + "<errors><error>blah</error></errors>" + Environment.NewLine + "</upload-attempt>";

			UploadAttemptResponse response = new UploadAttemptResponse(xml);
			Assert.IsFalse(response.WasSuccess);
			Assert.IsFalse(response.IsPending);
		}

		[Test()]
		public void UploadAttempXmlValidationFailureTest()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine + "<upload-attempt>" + Environment.NewLine + "<id>4048</id>" + Environment.NewLine + "<status>upload</status>" + Environment.NewLine + "<rows-uploaded>99</rows-uploaded>" + Environment.NewLine + "<uploaded-row-data><tbl_reg type=\"array\"></tbl_reg></uploaded-row-data>" + Environment.NewLine + "<row-errors type=\"array\"><row-error><level>1</level><level-description>Upload prevented until fixed</level-description><details>bad data</details></row-error></row-errors>" + Environment.NewLine + "<errors><error>blah</error></errors>" + Environment.NewLine + "</upload-attempt>";

			UploadAttemptResponse response = new UploadAttemptResponse(xml);
			Assert.IsFalse(response.WasSuccess);
			Assert.IsFalse(response.IsPending);

            Assert.AreEqual(response.ValidationErrors.Count, 1);
            ValidationError error = response.ValidationErrors[0];
            Assert.AreEqual(error.level, 1);
            Assert.AreEqual(error.level_description, "Upload prevented until fixed");
            Assert.AreEqual(error.details, "bad data");
		}

		[Test()]
		public void UploadAttempXmlInvalidResponseTest()
		{
			string xml = "";

			UploadAttemptResponse response = new UploadAttemptResponse(xml);
			Assert.IsFalse(response.WasSuccess);
			Assert.IsFalse(response.IsPending);
		}
	}
}

