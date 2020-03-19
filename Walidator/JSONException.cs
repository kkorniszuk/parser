using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Walidator
{
    public class JSONException : Exception
    {

        public JSONException()
        {

        }

        public JSONException(string message)
            : base(message)
        {

        }
    }
}