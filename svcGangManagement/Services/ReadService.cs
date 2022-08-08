using AutoMapper;
using svcGangManagement.Models;
using svcGangManagement.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svcGangManagement.Services
{
    public class ReadService : BaseService
    {
        public IEnumerable<SDivision> GetSDivision()
        {
            return railTestingEntities.SDivisions.Where(d => d.Active == true).ToList();
        }

        public IEnumerable<Gang_Type> GangTypePG()
        {
            return prodGangEntities.Gang_Type.ToList();
        }

        public IEnumerable<GangType> GetGangTypeMWS()
        {
            return mWSTRackEntities.GangTypes.ToList();
        }

        public IEnumerable<Gang> GetGangsPG()
        {
            return prodGangEntities.Gangs.ToList();
        }

        public IEnumerable<Gangs> GetGangsMWS()
        {
            return mWSTRackEntities.Gangs.ToList();
        }

        public IEnumerable<stpGetJobCodeInfo_Result> GetSupervisorByJobCode(string jobcode)
        {
            return cHRISEntities.stpGetJobCodeInfo(jobcode).ToList();
        }

        public IEnumerable<stpGetGangSupervisorInfo_Result> GetGangDetailsById(string id)
        {
            return prodGangEntities.stpGetGangSupervisorInfo(id).ToList();
        }
    }
}