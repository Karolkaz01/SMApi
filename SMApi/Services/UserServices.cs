using SMApi.Models;
using SMApi.ModelsDto;

namespace SMApi.Services
{
    public class UserServices
    {

        private readonly SMApiContext _context;

        public UserServices(SMApiContext context)
        {
            _context = context;
        }

        public List<User> Get() => _context.Users.ToList();

        public User? GetOne(int id) => _context.Users.Find(id);

        public User? GetByName(string Name) => _context.Users.FirstOrDefault(x => x.Name == Name);
      

        public User Add(string Name, Role AccountType)
        {
            var user = new User();

            user.Name = Name;
            user.AccountType = AccountType; 
            user.PasswordHash = new byte[0];
            user.PasswordSalt = new byte[0];

            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User Insert(string Name, Role AccountType, byte[] passwordHash, byte[] passwordSalt)
        {
            var user = new User();

            user.Name = Name;
            user.AccountType = AccountType;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User? Update(string Name, Role AccountType)
        {
            var user = GetByName(Name);
            if (user == null)
                return null;

            user.Name = Name;
            user.AccountType = AccountType;

            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }


        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return;

            _context.Remove(user);
            _context.SaveChanges();
        }

        /*
        private static List<User> ListOfUsers = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Krzyś Tomaszewski",
                AccountType = Role.Admin
            },
            new User
            {
                Id = 2,
                Name = "Baba Jaga",
                AccountType = Role.Employee
            },
            new User
            {
                Id = 3,
                Name = "Najman Kaczyński",
                AccountType = Role.Employee
            },
            new User
            {
                Id = 4,
                Name = "Roman Simiński",
                AccountType = Role.Admin
            }
        };


        static int nextId = 5;

        public static List<User> getAll() => ListOfUsers;

        public static User? get(int id) => ListOfUsers.Find(p => p.Id == id);

        public static void add(User user)
        {
            user.Id = nextId++;
            ListOfUsers.Add(user);
        }

        public static void update(User newUser)
        {
            var user = get(newUser.Id);
            if (user == null)
                return;

            user.Name = newUser.Name;
            user.AccountType = newUser.AccountType;
        }

        public static void delete(int id)
        {
            var user = get(id);
            if (user == null)
                return;

            ListOfUsers.Remove(user);
        }
        */
    }
}
