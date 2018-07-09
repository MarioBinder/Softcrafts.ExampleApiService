using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $rootnamespace$.Entities
{
    public class SemasSimpleBase
    {
        [Description("Der PrimaryKey.")]
        [Key]
        public long Id { get; set; }

        [Description("Die eindeutige Id zur Identifizierung oder Migration in andere Systeme. Die Erzeugung der Id findet auf der Datenbank statt.")]
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        public string IsDeleted { get; set; }
    }
}    