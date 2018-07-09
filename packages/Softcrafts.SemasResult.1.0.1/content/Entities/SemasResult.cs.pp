using System;

namespace $rootnamespace$.Entities
{
    public class SemasResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Errorcode { get; set; }

        public SemasResult(bool success, string message = "", string errorcode = "")
        {
            Success = success;
            Message = message;
            Errorcode = errorcode;
        }
    }

    public class SemasResult<T> : SemasResult
    {
        public T Data { get; set; }

        public SemasResult(bool success, string message = "", string errorcode = "") : base(success, message, errorcode)
        {
        }
        public SemasResult(bool success, T data, string message = "", string errorcode = "") : base(success, message, errorcode)
        {
            Data = data != null ? data : Activator.CreateInstance<T>();
        }
    }
}