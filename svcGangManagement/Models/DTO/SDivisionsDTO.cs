using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svcGangManagement.Models.DTO
{
    public class SDivisionsDto
    {
        public int DivisionID { get; set; }
        public string DivisionName { get; set; }
        public string DivisionCode { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}