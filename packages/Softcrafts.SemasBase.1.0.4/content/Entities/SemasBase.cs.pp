using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $rootnamespace$.Entities
{
    public class SemasBase : SemasSimpleBase
    {

        [Description("Legt fest ob ein Datensatz als geloescht behandelt werden soll. Geloeschte Datensaetze werden auf der Oberflaeche nicht angezeigt.")]
        public bool IsDeleted { get; set; }

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