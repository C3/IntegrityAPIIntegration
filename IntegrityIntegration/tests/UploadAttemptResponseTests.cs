
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;

namespace IntegrityAPITests
{
	public class UploadAttemptResponseTests
	{

		[Test()]
		public void UploadAttempXmlValidatedTest()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Constants.vbCrLf + "<upload-attempt>" + Constants.vbCrLf + "<id>4048</id>" + Constants.vbCrLf + "<status>upload</status>" + Constants.vbCrLf + "<rows-uploaded>99</rows-uploaded>" + Constants.vbCrLf + "<uploaded-row-data><tbl_reg type=\"array\"></tbl_reg></uploaded-row-data>" + Constants.vbCrLf + "<row-errors type=\"array\"></row-errors>" + Constants.vbCrLf + "<errors></errors>" + Constants.vbCrLf + "</upload-attempt>";

			UploadAttemptResponse response = new UploadAttemptResponse(xml);
			Assert.IsTrue(response.WasSuccess);
			Assert.IsFalse(response.IsPending);
			Assert.AreEqual(response.GetId, 4048);
		}

		[Test()]
		public void UploadAttempXmlUploadedTest()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Constants.vbCrLf + "<upload-attempt>" + Constants.vbCrLf + "<id>4048</id>" + Constants.vbCrLf + "<status>pending</status>" + Constants.vbCrLf + "<rows-uploaded>99</rows-uploaded>" + Constants.vbCrLf + "<uploaded-row-data><tbl_reg type=\"array\"></tbl_reg></uploaded-row-data>" + Constants.vbCrLf + "<row-errors type=\"array\"></row-errors>" + Constants.vbCrLf + "<errors></errors>" + Constants.vbCrLf + "</upload-attempt>";

			UploadAttemptResponse response = new UploadAttemptResponse(xml);
			Assert.IsTrue(response.WasSuccess);
			Assert.IsTrue(response.IsPending);
			Assert.AreEqual(response.GetId, 4048);
		}

		[Test()]
		public void UploadAttempXmlCreatedTest()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Constants.vbCrLf + "<upload-attempt>" + Constants.vbCrLf + "<id>4048</id>" + Constants.vbCrLf + "<status>validate</status>" + Constants.vbCrLf + "<rows-uploaded>99</rows-uploaded>" + Constants.vbCrLf + "<uploaded-row-data><tbl_reg type=\"array\"></tbl_reg></uploaded-row-data>" + Constants.vbCrLf + "<row-errors type=\"array\"></row-errors>" + Constants.vbCrLf + "<errors></errors>" + Constants.vbCrLf + "</upload-attempt>";

			UploadAttemptResponse response = new UploadAttemptResponse(xml);
			Assert.IsTrue(response.WasSuccess);
			Assert.IsFalse(response.IsPending);
			Assert.AreEqual(response.GetId, 4048);
		}

		[Test()]
		public void UploadAttempXmlCreatFailureTest()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Constants.vbCrLf + "<upload-attempt>" + Constants.vbCrLf + "<id></id>" + Constants.vbCrLf + "<status></status>" + Constants.vbCrLf + "<rows-uploaded>99</rows-uploaded>" + Constants.vbCrLf + "<uploaded-row-data><tbl_reg type=\"array\"></tbl_reg></uploaded-row-data>" + Constants.vbCrLf + "<row-errors type=\"array\"></row-errors>" + Constants.vbCrLf + "<errors><error>blah</error></errors>" + Constants.vbCrLf + "</upload-attempt>";

			UploadAttemptResponse response = new UploadAttemptResponse(xml);
			Assert.IsFalse(response.WasSuccess);
			Assert.IsFalse(response.IsPending);
		}

		[Test()]
		public void UploadAttempXmlValidationFailureTest()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Constants.vbCrLf + "<upload-attempt>" + Constants.vbCrLf + "<id>4048</id>" + Constants.vbCrLf + "<status>upload</status>" + Constants.vbCrLf + "<rows-uploaded>99</rows-uploaded>" + Constants.vbCrLf + "<uploaded-row-data><tbl_reg type=\"array\"></tbl_reg></uploaded-row-data>" + Constants.vbCrLf + "<row-errors type=\"array\"><row-error>bad data</row-error></row-errors>" + Constants.vbCrLf + "<errors><error>blah</error></errors>" + Constants.vbCrLf + "</upload-attempt>";

			UploadAttemptResponse response = new UploadAttemptResponse(xml);
			Assert.IsFalse(response.WasSuccess);
			Assert.IsFalse(response.IsPending);
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

