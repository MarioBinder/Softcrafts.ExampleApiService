using System;
using Softcrafts.Jobs.Entities.Base;

namespace Softcrafts.Jobs.Entities
{
    public class Job : SemasBase
    {
        public int JobTypeId { get; set; }
        public string JobName { get; set; }
        public string ExecutionTime { get; set; }
        public string JobArguments { get; set; }
        public bool InWork { get; set; }
        public bool Done { get; set; }
        public bool IsCanceled { get; set; }
        public Guid? EntityId { get; set; }
        public string EntityName { get; set; }
    }

    public class Settings : SemasBase
    {
        public string Propertyname { get; set; }
        public string PropertyValue { get; set; }
    }

    public class APIUser : SemasBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string InitVector { get; set; }
        public string SaltValue { get; set; }
        public string PassPhrase { get; set; }
        public bool IsAdmin { get; set; }
    }
}
