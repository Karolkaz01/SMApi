using SMApi.Models;
using SMApi.ModelsDto;

namespace SMApi.Services
{
    public class LocationServices
    {


        private readonly SMApiContext _context;

        public LocationServices(SMApiContext context)
        {
            _context = context;
        }


        public List<Location> Get() => _context.Locations.ToList();

        public Location? GetOne(int id) => _context.Locations.Find(id);

        public Location? Add(LocationDto locationDto)
        {
            var location = new Location();

            location.Name = locationDto.Name;

            _context.Locations.Add(location);
            _context.SaveChanges();
            return location;
        }

        public Location? Update(LocationDto locationDto, int id)
        {
            var location = _context.Locations.Find(id);
            if (location == null)
                return null;

            location.Name = locationDto.Name;

            _context.Locations.Update(location);
            _context.SaveChanges();

            return location;
        }


        public void Delete(int id)
        {
            var location = _context.Locations.Find(id);
            if (location == null)
                return;

            _context.Remove(location);
            _context.SaveChanges();
        }


        /*

        private static List<Location> ListOfLocations = new List<Location>
        {
            new Location
            {
                Id = 1,
                Desks = new List<Desk>
                {
                    DeskServices.get(1),
                    DeskServices.get(2),
                    DeskServices.get(3),
                }
            },
            new Location
            {
                Id = 2,
                Desks = new List<Desk>
                {
                    DeskServices.get(4),
                    DeskServices.get(5),
                    DeskServices.get(6),
                    DeskServices.get(7),
                }
            }
        };

        static int nextId = 3;


        public static List<Location> getAll() => ListOfLocations;

        public static Location? get(int id) => ListOfLocations.Find(p => p.Id == id);

        public static void add(Location location)
        {
            location.Id = nextId++;
            ListOfLocations.Add(location);
        }

        public static void update(Location newLocation)
        {
            var location = get(newLocation.Id);
            if (location == null)
                return;

            location.Desks = newLocation.Desks;
        }

        public static void delete(int id)
        {
            var location = get(id);
            if (location == null)
                return;

            ListOfLocations.Remove(location);
        }

        public static void addDesk(Desk desk,int id)
        {
            var location = get(id);
            if (location == null)
                return;

            location.Desks.Add(desk);
        }

        */
    }
}
