using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Softcrafts.Jobs.Entities.Base
{
    public class SemasSimpleBase
    {
        [Description("Der PrimaryKey.")]
        [Key]
        public long Id { get; set; }

        [Description("Die eindeutige Id zur Identifizierung oder Migration in andere Systeme. Die Erzeugung der Id findet auf der Datenbank statt.")]
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        public bool IsDeleted { get; set; }

        [NotMapped]
        public Link _links { get; set; } = new Link();
    }

    public class Link
    {
        public List<Action> _actions { get; set; } = new List<Action>();
        public Dictionary<string, string> _infos { get; set; } = new Dictionary<string, string>();
    }

    public class Action
    {
        public string type { get; set; }
        public string rel { get; set; }
        public string href { get; set; }
    }
}
