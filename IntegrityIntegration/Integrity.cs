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
using System.Linq;
using System.Diagnostics;
using IntegrityAPI;
using IntegrityIntegration;

public class Integrity
{
	private IntegrityHttpService _integrity_service;
	private Configuration _configuration;

	private IntegrityInterface _integrity_interface;

  /// <summary>
  /// Creates a new Integrity instance to query upon. Entry point to the API
  /// </summary>
  /// <param name="user">Username</param>
  /// <param name="password">Password</param>
  /// <param name="service_url">URL For Integrity instance, eg "http://integrity.myhost.com"</param>
  /// 
	public Integrity(string user, string password, string service_url)
	{
		_integrity_service = new IntegrityHttpService(service_url, user, password);
		_configuration = new Configuration();
		_configuration.BuildFromXml(_integrity_service.GetConfiguration());
		_integrity_interface = new IntegrityInterface(ref _integrity_service);
	}

  /// <summary>
  /// Create a new Search object with the provided qualifiers as parameters
  /// </summary>
  /// <param name="datasetName">Name of dataset</param>
  /// <param name="qualifier_selections">Qualifiers to search against as a dictionary of attribute name to list of qualifier values</param>
  /// <returns></returns>
  public Search NewSearch(string datasetName, IDictionary<string, IList<string>> qualifier_selections)
  {
    var config_dataset = _configuration.GetDataset(datasetName);
    var search_dataset = new IntegrityDataset();
    search_dataset.m_id = config_dataset.m_id;
    search_dataset.m_name = config_dataset.m_name;
    search_dataset.m_tableName = config_dataset.m_tableName;

    var qualifiers = new List<Qualifier>();

    foreach (var qualifier_selection in qualifier_selections)
    {
      qualifiers.Add(new Qualifier(qualifier_selection.Key, qualifier_selection.Value.ToArray()));
    }

    search_dataset.m_qualifiers = qualifiers;

    return new Search(search_dataset, _integrity_service);
  }

  /// <summary>
  /// Creates a new Search object that will search against all qualifiers the currently logged in user
  /// </summary>
  /// <param name="datasetName">Dataset name</param>
  /// <returns></returns>
    public Search NewSearch(string datasetName)
    {
        IntegrityDataset ds = _configuration.GetDataset(datasetName);
        return new Search(ds, _integrity_service);
    }

  /// <summary>
  /// Returns a list of names of datasets available to the currently logged in user
  /// </summary>
  /// <returns></returns>
    public IList<String> AvailableDatasets()
    {
      return _configuration.m_Datasets.Select(d => d.m_name).ToList();
    }

  /// <summary>
  /// Returns the qualifiers available to the current user for a dataset as a dictionary mapping attribute name to the lis of available qualifier values
  /// </summary>
  /// <param name="dataset">Name of dataset</param>
  /// <returns></returns>
    public Dictionary<string, IList<string>> AvailableQualifiers(string dataset)
    {
      var qualifiers =_configuration.GetQualifiersForDataset(_configuration.GetDataset(dataset).m_id);

      var qualifier_map = new Dictionary<string, IList<string>>();

      qualifiers.ForEach(q => qualifier_map.Add(q.AttributeName,q.Values));

      return qualifier_map;
      
    }

	public int ExhaustiveIncrementalDatasetUpload(int dataset_id, ref string payload)
	{
		UploadAttempt upload_attempt = new UploadAttempt(_configuration.GetDataset(dataset_id), _configuration.GetQualifiersForDataset(dataset_id), ref payload, "XML", UploadAttempt.Type.Incremental);

		UploadAttemptResponse created_status = default(UploadAttemptResponse);
		created_status = _integrity_interface.CreateUpload(ref upload_attempt);
		if (!created_status.WasSuccess) {
			return 0;
		}

		UploadAttemptResponse validated_status = default(UploadAttemptResponse);
		validated_status = _integrity_interface.ValidateUpload(created_status.GetId);
		if (!validated_status.WasSuccess) {
			return 0;
		}

		UploadAttemptResponse uploaded_status = default(UploadAttemptResponse);
		uploaded_status = _integrity_interface.Upload(validated_status.GetId);
		if (!uploaded_status.WasSuccess) {
			return 0;
		}

		return uploaded_status.GetId;
	}
}
