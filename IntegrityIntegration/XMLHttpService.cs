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
