using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace MVC.WEB.Provider
{
    public class Response
    {
        public Response(object Items = null, HttpStatusCode StatusCode = HttpStatusCode.OK, string StatusText = "", double TotalItems = 0)
        {
            items = Items;
            status = StatusCode;
            statusText = StatusText;
            totalItems = TotalItems;

        }

        //public Exception Exception { get; set; }
        //public string Message { get; set; }
        //public bool IsSuccess { get; set; }
        //public object Data { get; set; }
        //public HttpStatusCode status { get; set; }

        public object items { set; get; }
        public HttpStatusCode status { set; get; }
        public string statusText { get; set; }
        public double totalItems { get; set; }

    }


}