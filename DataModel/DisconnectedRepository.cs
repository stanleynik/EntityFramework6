﻿using NinjaDomain.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace NinjaDomain.DataModel
{
    public class DisconnectedRepository
    {
        public List<Ninja> GetNinjasWithClan()
        {
            using (var context = new NinjaContext())
            {
                return context.Ninjas.AsNoTracking().Include(x => x.Clan).ToList();
            }
        }

        public Ninja GetNinjaWithEquipment(int id)
        {
            using (var context = new NinjaContext())
            {
                return context.Ninjas.AsNoTracking().Include(x => x.EquipmenaddtOwned)
                    .FirstOrDefault(n => n.Id == id);
            }
        }

        public Ninja GetNinjaWithEquipmentAndClan(int id)
        {
            using (var context = new NinjaContext())
            {
                return context.Ninjas.AsNoTracking().Include(n => n.EquipmenaddtOwned)
                    .Include(n => n.Clan)
                    .FirstOrDefault(n => n.Id == id);
            }
        }

        public IEnumerable GetClanList()
        {
            using (var context = new NinjaContext())
            {
                return context.Clans.AsNoTracking().OrderBy(c => c.ClanName)
                    .Select(c => new { c.Id, c.ClanName }).ToList();
            }
        }

        public Ninja GetNinjaById(int id)
        {
            using (var context = new NinjaContext())
            {
                return context.Ninjas.Find(id);
            }
        }

        public void SaveUpdatedNinja(Ninja ninja)
        {
            using (var context = new NinjaContext())
            {
                context.Ninjas.Add(ninja); 
                context.SaveChanges();
            }
        }

        public void DeleteNinja(int ninjaId)
        {
            using (var context = new NinjaContext())
            {
                var ninja = context.Ninjas.Find(ninjaId);
                context.Entry(ninja).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void SaveNewEquipment(NinjaEquipment equipment, int ninjaId)
        {
            using (var context = new NinjaContext())
            {
                var ninja = context.Ninjas.Find(ninjaId);
                ninja.EquipmenaddtOwned.Add(equipment);

                context.SaveChanges();
            }
        }

        public void SaveUpdatedEquipment(NinjaEquipment equipment, int ninjaId)
        {
            using (var context = new NinjaContext())
            {
                var equipmentWithNinjaFromDatabase = context.Equipment
                    .Include(n => n.Ninja)
                    .FirstOrDefault(e => e.Id == equipment.Id);
                context.Entry(equipmentWithNinjaFromDatabase).CurrentValues.SetValues(equipment);
                context.SaveChanges();
            }
        }

    }
}
