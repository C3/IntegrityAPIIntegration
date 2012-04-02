
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.IO.Compression;
class XMLHTTPService
{

	private string _user;
	private string _password;

	private System.Net.WebClient _webclient;

	public XMLHTTPService(string user, string password)
	{
		_user = user;
		_password = password;
		_webclient = new System.Net.WebClient();
		_webclient.Headers.Set(System.Net.HttpRequestHeader.Accept, "application/xml");
		_webclient.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(user + ":" + password)));

	}

	public virtual string GetXML(string url)
	{
		HttpWebRequest req = null;
		req = BasicWebRequest(url);
		req.Method = "GET";

		WebResponse rsp = null;
		rsp = req.GetResponse();

		StreamReader reader = new StreamReader(rsp.GetResponseStream());
		return reader.ReadToEnd();
	}

	public virtual string PostXML(string url, string body)
	{
		HttpWebRequest req = null;
		req = BasicWebRequest(url);
		req.Method = "POST";

		StreamWriter writer = new StreamWriter(req.GetRequestStream());
		writer.Write(body);
		writer.Close();

		WebResponse rsp = null;
		rsp = req.GetResponse();

		req.GetRequestStream().Close();

		StreamReader reader = new StreamReader(rsp.GetResponseStream());
		return reader.ReadToEnd();
	}

	private HttpWebRequest BasicWebRequest(string url)
	{
		HttpWebRequest req = null;

		req = (HttpWebRequest)WebRequest.Create(url);
		req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
		req.AutomaticDecompression = DecompressionMethods.GZip;
		req.Timeout = 300000;
		req.ContentType = "application/xml";
		req.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(_user + ":" + _password)));

		return req;
	}

}
