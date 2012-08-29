
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;



using IntegrityAPI;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;

namespace IntegrityAPITests
{

	[TestFixture()]
	public class IntegrityHttpServiceTest
	{
		private class MockXMLHTTPService : XMLHTTPService
		{
			public string _last_query;
			public string _last_data;
			public MockXMLHTTPService() : base("", "")
			{
			}

			public override string GetXML(string url)
			{
				_last_query = url;
				return "GetXML";
			}

			public override string PostXML(string url, string data)
			{
				_last_query = url;
				_last_data = data;
				return "PostXML";
			}
		}

		IntegrityHttpService _instance;

		MockXMLHTTPService _mock_service;
		[SetUp()]
		public void TestSetup()
		{
			_instance = new IntegrityHttpService("url", "user", "password");
            _mock_service = new MockXMLHTTPService();
            XMLHTTPService _a = (XMLHTTPService)_mock_service;
            _instance.SetService(ref _a);
		}

		[Test()]
		public void GetConfigurationXml()
		{
			StringAssert.AreEqualIgnoringCase(_instance.GetConfiguration(), "GetXML");
			StringAssert.AreEqualIgnoringCase(_mock_service._last_query, "url/upload_attempts/new.xml");
		}

		[Test()]
		public void CreateUploadAttemptFromXML()
		{
			StringAssert.AreEqualIgnoringCase(_instance.CreateUploadAttempt("upload attempt data"), "PostXML");
			StringAssert.AreEqualIgnoringCase(_mock_service._last_query, "url/upload_attempts.xml");
			StringAssert.AreEqualIgnoringCase(_mock_service._last_data, "upload attempt data");
		}

		[Test()]
		public void ValidateUploadAttempt()
		{
			StringAssert.AreEqualIgnoringCase(_instance.ValidateUploadAttempt(7), "PostXML");
			StringAssert.AreEqualIgnoringCase(_mock_service._last_query, "url/upload_attempts/7/validate_file.xml");
		}

		[Test()]
		public void UploadUploadAttempt()
		{
			StringAssert.AreEqualIgnoringCase(_instance.UploadUploadAttempt(7), "PostXML");
			StringAssert.AreEqualIgnoringCase(_mock_service._last_query, "url/upload_attempts/7/upload.xml");
		}

		[Test()]
		public void GetUploadStatus()
		{
			StringAssert.AreEqualIgnoringCase(_instance.GetUploadStatus(7), "GetXML");
			StringAssert.AreEqualIgnoringCase(_mock_service._last_query, "url/upload_attempts/7.xml");
		}

        [Test()]
        public void GetSearchResultsTest()
        {
            _instance.GetSearchResults(10, "");
            StringAssert.AreEqualIgnoringCase(_mock_service._last_query, "url/datasets/10/search_results.xml?");
        }

        [Test()]
        public void GetSearchResultsWithParamsTest()
        {
            _instance.GetSearchResults(10, "page=1&per_page=500");
            StringAssert.AreEqualIgnoringCase(_mock_service._last_query, "url/datasets/10/search_results.xml?page=1&per_page=500");
        }

	}
}

