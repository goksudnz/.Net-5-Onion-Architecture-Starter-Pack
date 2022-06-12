// Copyrights(c) Charqe.io. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.General
{
    public class ServiceResult
    {
        public ServiceResult() {}

        public ServiceResult(bool status = true, string explanation = "")
        {
            Status = status;
            Explanation = explanation;
        }
        
        public bool Status { get; set; }
        public string Explanation { get; set; }
    }

    public class ServiceResultExt<T> : ServiceResult
    {
        public ServiceResultExt() { }

        public ServiceResultExt(T resultObject, bool status = true, string explanation = "")
        {
            Status = status;
            Explanation = explanation;
            ResultObject = resultObject;
        }

        public T ResultObject { get; set; }
    }

    public class ServiceResultScalarExt<T> : ServiceResultExt<T>
    {
        public ServiceResultScalarExt(T resultObject, int totalCount = 0, bool status = true, string explanation = "")
        {
            Status = status;
            ResultObject = resultObject;
            TotalCount = totalCount;
            Explanation = explanation;
        }
        public int TotalCount { get; set; }
    }

    public static class ServiceResultExtensions
    {
        public static ServiceResultExt<T> ToServiceResultExt<T>(this T resultObject) where T: class, new() 
            => new ServiceResultExt<T>(resultObject);

        public static ServiceResultScalarExt<T> ToServiceResultExt<T>(this T resultObject, int totalCount) where T : class
            => new ServiceResultScalarExt<T>(resultObject, totalCount);
    }
}