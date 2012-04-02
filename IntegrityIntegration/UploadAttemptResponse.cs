using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml;

namespace IntegrityAPI
{
    class UploadAttemptResponse
    {

        private int _id;
        private bool _was_success;

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
            }

            if (xmld.ChildNodes.Count == 0)
            {
                _was_success = false;
            }
            else if (xmld.SelectSingleNode("//errors").ChildNodes.Count > 0)
            {
                _was_success = false;
            }
            else if (xmld.SelectSingleNode("//row-errors").ChildNodes.Count > 0)
            {
                _was_success = false;
            }
            else
            {
                _was_success = true;
                _id = int.Parse(xmld.SelectSingleNode("//id").InnerText);
                if (xmld.SelectSingleNode("//status").InnerText == "pending")
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
