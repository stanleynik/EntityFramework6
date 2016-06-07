using NinjaDomain.Classes.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace NinjaDomain.Classes
{
    public class Ninja : IModificationHistory
    {
        public Ninja()
        { 
            EquipmenaddtOwned = new List<NinjaEquipment>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool ServedInOniwaban { get; set; }
        public Clan Clan { get; set; }
        public int ClanId { get; set; }
        public List<NinjaEquipment> EquipmenaddtOwned { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime DateModified { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsDirty { get; set; }
    }

    public class Clan : IModificationHistory
    {
        public Clan()
        { 
            Ninjas = new List<Ninja>();
        }
        public int Id { get; set; }
        public string ClanName { get; set; }
        public List<Ninja> Ninjas { get; set; }
  
        public DateTime DateModified { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsDirty { get; set; }
    }

    public class NinjaEquipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EquipmentType Type { get; set; }
        //public int NinjaId { get; set; }
        [Required]
        public Ninja Ninja { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDirty { get; set; }

    }

}
