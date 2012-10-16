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

    public UploadAttemptResponse WaitUntilValidated(int upload_attempt_id)
    {
        string[] validation_statuses = { "transferring", "pending_validation", "validating" };
        return WaitUntilNotStatus(upload_attempt_id, validation_statuses);
    }

    public UploadAttemptResponse WaitUntilUploaded(int upload_attempt_id)
    {
        string[] upload_statuses = { "pending_upload", "uploading" };
        return WaitUntilNotStatus(upload_attempt_id, upload_statuses);
    }

    private UploadAttemptResponse WaitUntilNotStatus(int upload_attempt_id, string[] statuses)
    {
        UploadAttemptResponse response = GetUploadStatus(upload_attempt_id);
        int attempts = 1;

        while (statuses.Contains(response.status))
        {
            if (attempts > 1200)
            {
                break;
            }
            System.Threading.Thread.Sleep(5000);
            response = GetUploadStatus(upload_attempt_id);
            attempts++;
        }

        return response;
    }
}
