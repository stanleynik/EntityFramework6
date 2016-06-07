using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());
            //InsertNinja();
            //InsertMultipleNinjas();
            //SimpleNinjaQueries();
            //QueryAndUpdateNinja();
            //QueryAndUpdateNinjaDisconnected();
            //RetrieveDataWithFind();
            //RetrieveDataWithStoredProcedure();
            //DeleteNinja();
            //DeleteNinjaViaStoredProcedure();
            //InsertNinjaWithEquipment();
            SimpleNinjaGraphQuery();
            Console.ReadKey();
        }

        private static void InsertMultipleNinjas()
        {
            var ninja1 = new Ninja
            {
                Name = "Leonardo",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1984, 1, 1),
                ClanId = 1
            };
            var ninja2 = new Ninja
            {
                Name = "Raphael",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1984, 1, 1),
                ClanId = 1
            };
            var ninja3 = new Ninja
            {
                Name = "Donatelo",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1984, 1, 1),
                ClanId = 1
            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.AddRange(new List<Ninja>{ ninja1, ninja2, ninja3 });
                context.SaveChanges();
            }
        }
        private static void InsertNinja()
        {
            var ninja = new Ninja
            {
                Name = "JulieSan",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1980, 1, 1),
                ClanId = 1
            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }
        }
        private static void SimpleNinjaQueries()
        {
            using (var context = new NinjaContext())
            {
                var ninjas = context.Ninjas.ToList();
               foreach(var ninja in ninjas)
               {
                   Console.WriteLine(ninja.Name);
               }
            }
        }
        private static void QueryAndUpdateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);
                context.SaveChanges();
            }
        }
        private static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Add(ninja);
                context.Entry(ninja).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        private static void RetrieveDataWithFind()
        {
            var keyval = 4;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find#1:" + ninja.Name);

                var someNinja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find#2:" + someNinja.Name);
                ninja = null;
            }
        }
        private static void RetrieveDataWithStoredProcedure()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas.SqlQuery("exec GetOldNinjas");
                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }
            }
        }
        private static void DeleteNinja()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Remove(ninja);
                context.Entry(ninja).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
        private static void DeleteNinjaWithKeyValie()
        {
            var keyval = 1;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);
                context.Ninjas.Remove(ninja);
                context.SaveChanges();
            }
        }
        private static void DeleteNinjaViaStoredProcedure()
        {
            var keyval = 3;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Database.ExecuteSqlCommand("exec DeleteNinjaViaId {0}", keyval);
            }
        }
        private static void InsertNinjaWithEquipment()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = new Ninja
                {
                    Name = "Kacy Catanzaro",
                    ServedInOniwaban = false,
                    DateOfBirth = new DateTime(1990,1,14),
                    ClanId = 1
                };
                var muscles = new NinjaEquipment
                {
                    Name = "Muscles",
                    Type = EquipmentType.Tool,
                };
                var spunk = new NinjaEquipment { 
                    Name = "Spunk",
                    Type = EquipmentType.Weapon,
                };
                context.Ninjas.Add(ninja);
                ninja.EquipmenaddtOwned.Add(muscles);
                ninja.EquipmenaddtOwned.Add(spunk);
                context.SaveChanges();
            }
        }

        private static void SimpleNinjaGraphQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                //Use include to eager loading
                //var ninja = context.Ninjas.Include(n => n.EquipmenaddtOwned)
                //              .FirstOrDefault(n => n.Name.StartsWith("Kacy"));
               
                //This way for explicit loading
                var ninja = context.Ninjas.FirstOrDefault(n => n.Name.StartsWith("Kacy"));
                Console.WriteLine("Ninja Retrieved:" + ninja.Name);
                //context.Entry(ninja).Collection(n => n.EquipmenaddtOwned).Load();
                //Lazy Load Set Navegation Property as Virtal (EquipmentaddtOwned)
                Console.WriteLine("Ninja Equipment Count: {0}", ninja.EquipmenaddtOwned.Count());
            }
        }

        private static void ProjectionQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas
                    .Select(n => new { n.Name, n.DateOfBirth, n.EquipmenaddtOwned })
                    .ToList();
            }
        }

    }
}
