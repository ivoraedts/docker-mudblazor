using Microsoft.EntityFrameworkCore;

namespace MyApplication.Data
{
    public class MudBlazorDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public MudBlazorDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //connect to postgress with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("MudBlazorDatabase"));
        }

        public DbSet<CalendarEvent> CalendarEvents {get; set;}
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMudBlazorDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MudBlazorDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("MudBlazorDatabase"))
            );

            return services;
        }
    }
}