using System;
using System.Collections.Generic;

namespace TWP_API_Payroll.Helpers
{
    public class ApiResponse
    {
        public string statusCode{get;set;}
        public object message{get;set;}
        public IEnumerable<string> error{get;set;}
        public object data{get;set;}

    }
}