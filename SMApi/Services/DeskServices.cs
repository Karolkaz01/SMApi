using SMApi.Models;
using SMApi.Data;
using SMApi.ModelsDto;

namespace SMApi.Services
{
    public class DeskServices
    {

        private readonly SMApiContext _context;

        public DeskServices(SMApiContext context)
        {
            _context = context;
        }

        public List<Desk> Get() => _context.Desks.ToList();

        public Desk? GetOne(int id) => _context.Desks.Find(id);

        public List<Desk> GetFromLocation(int id)
        {
            var desks = _context.Desks
                .Where(d => d.LocationId == id)
                .ToList();

            return desks;
        }

        public Desk? Add(DeskDto deskDto)
        {
            var location = _context.Locations.Find(deskDto.LocationId);
            if (location == null)
                return null;

            var desk = new Desk();

            desk.IsAvailable = deskDto.IsAvailable;
            desk.LocationId = deskDto.LocationId;

            _context.Desks.Add(desk);
            _context.SaveChanges();
            return desk;
        }

        public Desk? Update(DeskDto deskDto, int id)
        {
            var desk = _context.Desks.Find(id);
            if (desk == null)
                return null;

            desk.LocationId = deskDto.LocationId;
            desk.IsAvailable = deskDto.IsAvailable;

            _context.Desks.Update(desk);
            _context.SaveChanges();

            return desk;
        }

        


        public void Delete(int id)
        {
            var desk = _context.Desks.Find(id);
            if (desk == null )
                return;

            _context.Remove(desk);
            _context.SaveChanges();
        }
        

        /*
        private static List<Desk> ListOfDesks = new List<Desk>
        {
            new Desk {
                Id = 1,
                IsAvailable = true
            },
            new Desk {
                Id=2,
                IsAvailable = true
            },
            new Desk {
                Id=3,
                IsAvailable = true
            },
            new Desk {
                Id=4,
                IsAvailable = true
            },
            new Desk {
                Id=5,
                IsAvailable = true
            },
            new Desk {
                Id=6,
                IsAvailable = true
            },
            new Desk {
                Id=7,
                IsAvailable = true
            }
        };

        private readonly SMApiContext _context;

        static int nextId = 8;



        public DeskServices(SMApiContext context)
        {
            _context = context;
        }

        public static List<Desk> getAll() => ListOfDesks;

        public static Desk? get(int id) =>  ListOfDesks.Find(p => p.Id == id);

        public void add(Desk desk)
        {
            _context.Desks.Add(desk);
            _context.SaveChanges();
            //desk.Id = nextId++;
            //ListOfDesks.Add(desk);
        }

        public static void update(Desk newDesk)
        {
            var desk = get(newDesk.Id);
            if (desk == null)
                return;

            desk.IsAvailable = newDesk.IsAvailable;
            
        }

        public static void delete(int id)
        {
            var desk = get(id);
            if (desk == null)
                return;

            ListOfDesks.Remove(desk);
        }
        */

    }
}
