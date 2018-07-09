using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Softcrafts.Jobs.Entities.Base
{
    public class SemasBase : SemasSimpleBase
    {
        [Description("Repaesentiert den User der den Datensatz angelegt hat.")]
        [Required]
        public string CreatedUser { get; set; }

        [Description("Repaesentiert das Datum an dem der Datensatz angelegt wurde.")]
        public DateTime CreatedDate { get; set; }

        [Description("Repaesentiert den User der den Datensatz zuletzt geaendert hat.")]
        public string UpdatedUser { get; set; }

        [Description("Repaesentiert das Datum an dem der Datensatz zuletzt geaendert wurde.")]
        public DateTime? UpdatedDate { get; set; }
    }
}
