
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using IntegrityAPI;

class IntegrityInterface
{

	private IntegrityHttpService _service;
	public IntegrityInterface(ref IntegrityHttpService service)
	{
		_service = service;
	}

	public UploadAttemptResponse CreateUpload(ref UploadAttempt upload_attempt)
	{
		return new UploadAttemptResponse(_service.CreateUploadAttempt(upload_attempt.BuildAttemptXml()));
	}

	public UploadAttemptResponse ValidateUpload(int upload_attempt_id)
	{
		return new UploadAttemptResponse(_service.ValidateUploadAttempt(upload_attempt_id));
	}

	public UploadAttemptResponse Upload(int upload_attempt_id)
	{
		return new UploadAttemptResponse(_service.UploadUploadAttempt(upload_attempt_id));
	}

	public UploadAttemptResponse GetUploadStatus(int upload_attempt_id)
	{
		return new UploadAttemptResponse(_service.GetUploadStatus(upload_attempt_id));
	}
}
