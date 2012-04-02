
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using IntegrityAPI;

public class Integrity
{
	private IntegrityHttpService _integrity_service;
	private Configuration _configuration;

	private IntegrityInterface _integrity_interface;
	public Integrity(string user, string password, string service_url)
	{
		_integrity_service = new IntegrityHttpService(service_url, user, password);
		_configuration = new Configuration();
		_configuration.BuildFromXml(_integrity_service.GetConfiguration());
		_integrity_interface = new IntegrityInterface(ref _integrity_service);
	}

	public int ExhaustiveIncrementalDatasetUpload(int dataset_id, ref string payload)
	{
		UploadAttempt upload_attempt = new UploadAttempt(_configuration.GetDataset(dataset_id), _configuration.GetQualifiersForDataset(dataset_id), ref payload, "XML");

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
