using svcGangManagement.Models;
using svcGangManagement.Models.DTO;
using svcGangManagement.Utility;
using System;
using AutoMapper;

namespace svcGangManagement.Services
{
    public class ErrorService : BaseService
    {
        public ErrorLogDto Log(Error_Log item)
        {
            try
            {
                errorLoggerEntities.Error_Log.Add(item);
                errorLoggerEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Email.SendError(ex);
            }

            Email.SendError(item);
            return Mapper.Map<ErrorLogDto>(item);
        }
    }
}