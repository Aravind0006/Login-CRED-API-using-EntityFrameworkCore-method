Quick Guide to Create an API Using ASP.NET Core Web API with Entity Framework Core method (Ef)

Step 1 : Create a New ASP.NET Core Web API Project (Visual studio 22 -> ASP.NET Core Web API -> Project Name -> Next-> Project Created)

Step 2 : Add the necessary NuGet packages (Tools - > NuGet packages Manager -> NuGet packages ->
 package mamange console -> Install -> dotnet add package Microsoft.EntityFrameworkCore.SqlServer and dotnet add package Microsoft.EntityFrameworkCore.Tools) or 
manage nuget package for solution -> Browse -> Install - >  

1.Microsoft.EntityFrameworkCore.SqlServer & 
2.Microsoft.EntityFrameworkCore.Tools )

Step 3: Create a folder DbContext and create a file DbContext.cs

using Full_stack_Reg___Login_CRED_API.Model;
using Microsoft.EntityFrameworkCore;
//use from here --
public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}
//to here --
Step 4: Create a Model Using User.cs
using System.ComponentModel.DataAnnotations;

namespace Full_stack_Reg___Login_CRED_API.Model
{
//from here ---
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } // In a real app, store hashed passwords

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
    }

    public class Login
    {
        [Required]
        [MaxLength(100)]
        public string Password { get; set; } // In a real app, store hashed passwords

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}

// to here --

Step 5 : Go to program.cs and paste this 
(// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<DataBaseConnection>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")));)

Step 6 : Go to appsettings.json and paste this 
"ConnectionStrings": {
    "SQLConnection": "Server=DESKTOP-AKRADV9;Database=TestDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;"
}
 

Step 7 : Go to controller folder -> Create a Controller -> ( Controller Folder -> Add -> Controller -> Select Api -> Empty -> Add -> Change Name -> Next to controller created ) and use like this

example : (
using Full_stack_Reg___Login_CRED_API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Full_stack_Reg___Login_CRED_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UsersController(MyDbContext context)
        {
            _context = context;
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                return BadRequest("Username already exists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // Login a user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok("Login successful."); // In a real app, return a JWT token
        }

        // Get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // Get a user by id
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // Update a user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Delete a user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
)

Step 8 : Apply Migrations and Update the Database
Go to package manager console and run dotnet ef migrations add InitialCreate and dotnet ef database update

Step 9 : Run to see the api
