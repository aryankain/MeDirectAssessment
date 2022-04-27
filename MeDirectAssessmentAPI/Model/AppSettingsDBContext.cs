using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeDirectAssessmentAPI.Model
{    
    public class AppSettingsDBContext : DbContext
    {
        public AppSettingsDBContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<AppSettings> AppSettings { get; set; }     
    }

    public class AppSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AppSettingKey { get; set; }
        public string AppSettingValue { get; set; }
    }
}
