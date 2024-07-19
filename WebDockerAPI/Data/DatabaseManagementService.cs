using Microsoft.EntityFrameworkCore;

namespace WebDockerAPI.Data
{
    public static class DatabaseManagementService
    {
        public static void MigrationInitialize(IApplicationBuilder app)
        {
            using(var service = app.ApplicationServices.CreateScope())
            {
                var ser = service.ServiceProvider.GetService<DataContext>();
                var db = ser.Database;
                db.Migrate();
            }
        }
    }
}
