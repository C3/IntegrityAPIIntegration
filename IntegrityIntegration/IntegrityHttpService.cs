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

public interface IIntegrityHttpService
{
    string GetSearchResults(int datasetId, string queryString);
}

class IntegrityHttpService : IIntegrityHttpService
{

	private string _base_upload_url, _service_address;

	private XMLHTTPService _service;

	public IntegrityHttpService(string service_address, string user, string password)
	{
		_base_upload_url = service_address + "/upload_attempts";
        _service_address = service_address;
		_service = new XMLHTTPService(user, password);
	}

	public string GetConfiguration()
	{
		return _service.GetXML(_base_upload_url + "/new.xml");
	}

	public void SetService(ref XMLHTTPService service)
	{
		_service = service;
	}

	public string CreateUploadAttempt(string upload_creation_xml)
	{
		return _service.PostXML(_base_upload_url + ".xml", upload_creation_xml);
	}

	public string ValidateUploadAttempt(int upload_attempt_id)
	{
		return _service.PostXML(_base_upload_url + "/" + upload_attempt_id + "/validate_file.xml", "");
	}

	public string UploadUploadAttempt(int upload_attempt_id)
	{
		return _service.PostXML(_base_upload_url + "/" + upload_attempt_id + "/upload.xml", "");
	}

	public string GetUploadStatus(int upload_attempt_id)
	{
		return _service.GetXML(_base_upload_url + "/" + upload_attempt_id + ".xml");
	}

    public string GetSearchResults(int datasetId, string queryString)
    {
        return _service.GetXML(string.Format("{0}/datasets/{1}/search_results.xml?{2}", _service_address, datasetId, queryString));
    }

}
