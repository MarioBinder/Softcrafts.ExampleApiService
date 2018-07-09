using System;

namespace Softcrafts.Jobs.Entities.Base
{
    public class SemasSprachreferenz : SemasSimpleBase
    {
        public Guid ReferenzId { get; set; }
        public string Original { get; set; }
        public string Sprache { get; set; }
        public string Value { get; set; }
    }
}
