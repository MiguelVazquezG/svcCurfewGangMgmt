using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svcGangManagement.Models.DTO
{
    public class ErrorLogDto
    {
        public int ID { get; set; }
        public string App_Name { get; set; }
        public string Source { get; set; }
        public string Method { get; set; }
        public DateTime Error_Date { get; set; }
        public DateTime Error_Time { get; set; }
        public string Computer { get; set; }
        public string Error_Message { get; set; }
        public string Stack_Trace { get; set; }
    }
}