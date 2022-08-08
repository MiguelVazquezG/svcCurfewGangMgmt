using svcGangManagement.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svcGangManagement.Services
{
    public class CreateService : BaseService
    {
        public bool Post(List<NewGang> gangs)
        {
            return true;
        }

        public bool EditGang(List<NewGang> gangs)
        {
            return true;
        }

    }
}