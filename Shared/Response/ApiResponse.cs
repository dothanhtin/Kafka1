using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Response
{
    public class ApiResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public ApiResponse() { }
        public ApiResponse(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
        public ApiResponse(int code, string message, object data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }
    }
}
