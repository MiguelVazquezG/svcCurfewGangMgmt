//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace svcGangManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Error_Log
    {
        public int ID { get; set; }
        public string App_Name { get; set; }
        public string Source { get; set; }
        public string Method { get; set; }
        public System.DateTime Error_Date { get; set; }
        public System.DateTime Error_Time { get; set; }
        public string Computer { get; set; }
        public string Error_Message { get; set; }
        public string Stack_Trace { get; set; }
    }
}
