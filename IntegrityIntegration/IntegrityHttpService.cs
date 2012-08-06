
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

public interface IIntegrityHttpService
{
    string GetSearchResults(int datasetId, string queryString);
}

class IntegrityHttpService : IIntegrityHttpService
{

	private string _base_url;

	private XMLHTTPService _service;

	public IntegrityHttpService(string service_address, string user, string password)
	{
		_base_url = service_address + "/upload_attempts";
		_service = new XMLHTTPService(user, password);
	}

	public string GetConfiguration()
	{
		return _service.GetXML(_base_url + "/new.xml");
	}

	public void SetService(ref XMLHTTPService service)
	{
		_service = service;
	}

	public string CreateUploadAttempt(string upload_creation_xml)
	{
		return _service.PostXML(_base_url + ".xml", upload_creation_xml);
	}

	public string ValidateUploadAttempt(int upload_attempt_id)
	{
		return _service.PostXML(_base_url + "/" + upload_attempt_id + "/validate_file.xml", "");
	}

	public string UploadUploadAttempt(int upload_attempt_id)
	{
		return _service.PostXML(_base_url + "/" + upload_attempt_id + "/upload.xml", "");
	}

	public string GetUploadStatus(int upload_attempt_id)
	{
		return _service.GetXML(_base_url + "/" + upload_attempt_id + ".xml");
	}

    public string GetSearchResults(int datasetId, string queryString)
    {
        return _service.GetXML(string.Format("{0}/datasets/{1}/search_results.xml?", _base_url, datasetId, queryString));
    }

}
