Quick Guide to Create an API Using ASP.NET Core Web API with Entity Framework Core method (Ef)

Step 1 : Create a New ASP.NET Core Web API Project (Visual studio 22 -> ASP.NET Core Web API -> Project Name -> Next-> Project Created)

Step 2 : Add the necessary NuGet packages (Tools - > NuGet packages Manager -> NuGet packages ->
 package mamange console -> Install -> dotnet add package Microsoft.EntityFrameworkCore.SqlServer and dotnet add package Microsoft.EntityFrameworkCore.Tools) or 
manage nuget package for solution -> Browse -> Install - > Microsoft.EntityFrameworkCore.SqlServer & Microsoft.EntityFrameworkCore.Tools )

Step 3: Create a folder DbContext and create a file DbContext.cs
(using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public DbSet<MyModel> MyModels { get; set; }
}

public class MyModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}) //paste this like

Step 4 : Go to program.cs and paste this 
(// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<DataBaseConnection>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")));)

Step 5 : Go to appsettings.json and paste this 
"ConnectionStrings": {
    "SQLConnection": "Server=DESKTOP-AKRADV9;Database=TestDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;"
}


Step 6 : Go to controller folder -> Create a Controller -> ( Controller Folder -> Add -> Controller -> Select Api -> Empty -> Add -> Change Name -> Next to controller created ) and use like this

example : (
[Route("api/[controller]")]
[ApiController]
public class MyModelsController : ControllerBase
{
    private readonly MyDbContext _context;

    public MyModelsController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MyModel>>> GetMyModels()
    {
        return await _context.MyModels.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MyModel>> GetMyModel(int id)
    {
        var model = await _context.MyModels.FindAsync(id);
        if (model == null)
        {
            return NotFound();
        }
        return model;
    }

    [HttpPost]
    public async Task<ActionResult<MyModel>> PostMyModel(MyModel model)
    {
        _context.MyModels.Add(model);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMyModel), new { id = model.Id }, model);
    }
}
)

Step 7 : Apply Migrations and Update the Database
Go to package manager console and run dotnet ef migrations add InitialCreate and dotnet ef database update

Step 8 : Run to see the api
