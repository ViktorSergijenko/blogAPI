using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Helpers
{
    public class ProjectExceptions : Exception
    {
        #region Error shortcuts

        public static ProjectExceptions NotFound { get { return new ProjectExceptions("NotFound", StatusCodes.Status404NotFound); } }
        public static ProjectExceptions BadRequest { get { return new ProjectExceptions("BadRequest", StatusCodes.Status400BadRequest); } }
        public static ProjectExceptions Forbidden { get { return new ProjectExceptions("Forbidden", StatusCodes.Status403Forbidden); } }
        public static ProjectExceptions UpdateConcurrency { get { return new ProjectExceptions("TheDatabaseRecordHasBeenChangedBySomeone", StatusCodes.Status409Conflict); } }
        public static ProjectExceptions InternalServerError { get { return new ProjectExceptions("UndefinedInternalServerError", StatusCodes.Status500InternalServerError); } }

        #endregion Error shortcuts

        public ProjectExceptions(string message, int code = StatusCodes.Status400BadRequest)
           : base(message)
        {
            Code = code;
        }

        public int Code { get; set; }

        #region Helpers

        /// <summary>
        /// If object is null - throw 404 not found error
        /// </summary>
        /// <param name="entity">Object, which is being inspected</param>
        public static void ThrowNotFoundIfNull(object entity)
        {
            if (entity == null) { throw NotFound; }
        }

        /// <summary>
        /// If object is null - throw 400 bad request error
        /// </summary>
        /// <param name="entity">Object, which is being inspected</param>
        public static void ThrowBadRequestIfNull(object entity)
        {
            if (entity == null) { throw BadRequest; }
        }

        #endregion Helpers
    }

    public class ValidationException : Exception
    {
        public int Code { get; set; }

        public ValidationException(ModelStateDictionary modelState, int code = StatusCodes.Status400BadRequest)
            // Not sure if this works fine. Will see later.
            : base(JsonConvert.SerializeObject(modelState.Values))
        {
            Code = code;
        }
    }

    public class HMGuruExceptionVM
    {
        public string Message { get; set; }
    }
}


