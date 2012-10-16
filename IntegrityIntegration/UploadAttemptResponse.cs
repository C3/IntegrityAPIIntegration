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
using System.Xml;

namespace IntegrityAPI
{
    public class UploadAttemptResponse
    {

        private int _id;
        private bool _was_success;
        public string status { get; set; }

        private bool _is_pending;
        public int GetId
        {
            get{ return _id; }
        }

        public bool WasSuccess
        {
            get{ return _was_success; }
        }

        public bool IsPending
        {
            get { return _is_pending; }
        }
        public List<ValidationError> ValidationErrors = new List<ValidationError>();

        public UploadAttemptResponse(string responseXml)
        {
            XmlDocument xmld = new XmlDocument();

            try
            {
                xmld.LoadXml(responseXml);
            }
            catch (Exception e)
            {
                _was_success = false;
                _is_pending = false;
                return;
            }

            this.status = xmld.SelectSingleNode("//status").InnerText;

            if ((xmld.ChildNodes.Count == 0) || (xmld.SelectSingleNode("//errors").ChildNodes.Count > 0) || (xmld.SelectSingleNode("//row-errors").ChildNodes.Count > 0))
            {
                _was_success = false;

                foreach (XmlNode node in xmld.SelectNodes("//row-error"))
                {
                    int level = int.Parse(node.SelectSingleNode("level").InnerText);
                    string level_description = node.SelectSingleNode("level-description").InnerText;
                    string details = node.SelectSingleNode("details").InnerText;
                    this.ValidationErrors.Add(new ValidationError { level = level, level_description = level_description, details = details });
                }
            }
            else
            {
                _was_success = true;
                _id = int.Parse(xmld.SelectSingleNode("//id").InnerText);
                if (status == "pending")
                {
                    _is_pending = true;
                }
                else
                {
                    _is_pending = false;
                }
            }
        }

    }
}
