
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
	public class ConfigurationTest
	{
		[SetUp()]
		public void TestSetup()
		{
		}

		[Test()]
		public void EmptyConfiguration()
		{
			Configuration _configuration = new Configuration();
			_configuration.BuildFromXml("<user-access-group-authorisation><datasets type=\"array\"/></user-access-group-authorisation>");
			Assert.AreEqual(_configuration.m_Datasets.Count, 0);
		}

		[Test()]
		public void SingleDatasetConfiguration()
		{
			Configuration _configuration = new Configuration();
			_configuration.BuildFromXml("<user-access-group-authorisation><datasets type=\"array\">" + "<dataset><id>9</id><name>Dataset name</name><is-bulk-allowed>false</is-bulk-allowed>" + "<is-incremental-allowed>true</is-incremental-allowed><table-name>dataset_name</table-name>" + "<qualifiers type=\"array\"/></dataset></datasets></user-access-group-authorisation>");
			Assert.AreEqual(_configuration.m_Datasets.Count, 1);
		}

        [Test()]
        public void GetDatasetViaId()
        {
            Configuration configuration = new Configuration();
            string xmlConfig = @"<user-access-group-authorisation><datasets type=""array"">
                    <dataset><id>9</id>
                    <name>people</name>
                    <is-bulk-allowed>false</is-bulk-allowed>
                    <is-incremental-allowed>true</is-incremental-allowed>
                    <table-name>dataset_name</table-name>
                    <qualifiers type=""array""/></dataset>
                </datasets></user-access-group-authorisation>";
            configuration.BuildFromXml(xmlConfig);
            IntegrityDataset dataset = configuration.GetDataset("people");
            Assert.AreEqual(9, dataset.m_id);
        }

        [Test()]
        public void GetDatasetViaName()
        {
            Configuration configuration = new Configuration();
            string xmlConfig = @"<user-access-group-authorisation><datasets type=""array"">
                    <dataset><id>9</id>
                    <name>people</name>
                    <is-bulk-allowed>false</is-bulk-allowed>
                    <is-incremental-allowed>true</is-incremental-allowed>
                    <table-name>dataset_name</table-name>
                    <qualifiers type=""array""/></dataset>
                </datasets></user-access-group-authorisation>";
            configuration.BuildFromXml(xmlConfig);
            IntegrityDataset dataset = configuration.GetDataset("people");
            Assert.AreEqual("people", dataset.m_name);
        }

        [Test()]
        [ExpectedException(typeof(Exception),
            ExpectedMessage="No data set named 'staff' found")]
        public void GetDatasetViaInvalidName()
        {
            Configuration configuration = new Configuration();
            string xmlConfig = @"<user-access-group-authorisation><datasets type=""array"">
                    <dataset><id>9</id>
                    <name>people</name>
                    <is-bulk-allowed>false</is-bulk-allowed>
                    <is-incremental-allowed>true</is-incremental-allowed>
                    <table-name>dataset_name</table-name>
                    <qualifiers type=""array""/></dataset>
                </datasets></user-access-group-authorisation>";

            configuration.BuildFromXml(xmlConfig);
            configuration.GetDataset("staff");
        }

        [Test()]
        [ExpectedException(typeof(Exception),
            ExpectedMessage = "No data set with id '10' found")]
        public void GetDatasetViaInvalidId()
        {
            Configuration configuration = new Configuration();
            string xmlConfig = @"<user-access-group-authorisation><datasets type=""array"">
                    <dataset><id>9</id>
                    <name>people</name>
                    <is-bulk-allowed>false</is-bulk-allowed>
                    <is-incremental-allowed>true</is-incremental-allowed>
                    <table-name>dataset_name</table-name>
                    <qualifiers type=""array""/></dataset>
                </datasets></user-access-group-authorisation>";

            configuration.BuildFromXml(xmlConfig);
            configuration.GetDataset(10);
        }

        [Test()]
        public void GetDatasetWithFormats()
        {
            Configuration _configuration = new Configuration();
            string xmlConfig = @"<user-access-group-authorisation>
                    <datasets type=""array"">
                        <dataset><id>9</id>
                            <name>people</name>
                            <is-bulk-allowed>false</is-bulk-allowed>
                            <is-incremental-allowed>true</is-incremental-allowed>
                            <table-name>dataset_name</table-name>
                            <qualifiers type=""array""/>
                            <dataset-formats type=""array"">
                                <dataset-format>
                                    <name>Countries CSV</name>
                                    <parser-type>CSV</parser-type>
                                </dataset-format>
                                <dataset-format>
                                    <name>Countries XML</name>
                                    <parser-type>XML</parser-type>
                                </dataset-format>
                            </dataset-formats>
                        </dataset>
                    </datasets>
                </user-access-group-authorisation>";
            _configuration.BuildFromXml(xmlConfig);
            IntegrityDataset _dataset = _configuration.GetDataset(9);
            
            Assert.AreEqual(_dataset.m_dataset_formats.Count, 2);

            DatasetFormat csv_format = (DatasetFormat)_dataset.m_dataset_formats.Find(c => c.name == "Countries CSV");
            Assert.NotNull(csv_format);
            Assert.AreEqual(csv_format.type, DatasetFormat.Type.CSV);

            DatasetFormat xml_format = (DatasetFormat)_dataset.m_dataset_formats.Find(c => c.name == "Countries XML");
            Assert.NotNull(xml_format);
            Assert.AreEqual(xml_format.type, DatasetFormat.Type.XML);
        }

        [Test()]
        public void GetDatasetWithFormatsWithLowercaseParserTypes()
        {
            Configuration _configuration = new Configuration();
            string xmlConfig = @"<user-access-group-authorisation>
                    <datasets type=""array"">
                        <dataset><id>9</id>
                            <name>people</name>
                            <is-bulk-allowed>false</is-bulk-allowed>
                            <is-incremental-allowed>true</is-incremental-allowed>
                            <table-name>dataset_name</table-name>
                            <qualifiers type=""array""/>
                            <dataset-formats type=""array"">
                                <dataset-format>
                                    <name>Countries CSV</name>
                                    <parser-type>csv</parser-type>
                                </dataset-format>
                                <dataset-format>
                                    <name>Countries XML</name>
                                    <parser-type>xml</parser-type>
                                </dataset-format>
                            </dataset-formats>
                        </dataset>
                    </datasets>
                </user-access-group-authorisation>";
            _configuration.BuildFromXml(xmlConfig);
            IntegrityDataset _dataset = _configuration.GetDataset(9);

            Assert.AreEqual(_dataset.m_dataset_formats.Count, 2);

            DatasetFormat csv_format = (DatasetFormat)_dataset.m_dataset_formats.Find(c => c.name == "Countries CSV");
            Assert.NotNull(csv_format);
            Assert.AreEqual(csv_format.type, DatasetFormat.Type.CSV);

            DatasetFormat xml_format = (DatasetFormat)_dataset.m_dataset_formats.Find(c => c.name == "Countries XML");
            Assert.NotNull(xml_format);
            Assert.AreEqual(xml_format.type, DatasetFormat.Type.XML);
        }

	}
}

