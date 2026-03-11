using System.ComponentModel.DataAnnotations;

namespace EmployeeMennage.AdDbContext
{
    public class RoleTBL
    {
        [Key]
        public int roleId { get; set; }

        [Required]
        public string roleName { get; set; } = string.Empty;
    }
}
