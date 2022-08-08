using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace svcGangManagement.Utility
{
    public class JsonResponse<T>
    {
        [DataMember]
        public string Message = "";

        [DataMember]
        public bool Success = false;

        [DataMember]
        public long Timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);

        [DataMember]
        public T Result = default(T);

        public List<Exception> Errors = new List<Exception>();

        public void addException(Exception e)
        {
            Errors.Add(e);
        }

        public void addResult(T result)
        {
            Result = result;
        }

        public void processResults()
        {
            Success = !Errors.Any();
            Message = Errors.FirstOrDefault()?.Message ?? "";
        }
    }
}