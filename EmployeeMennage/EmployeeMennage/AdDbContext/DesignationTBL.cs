using System.ComponentModel.DataAnnotations;

namespace EmployeeMennage.AdDbContext
{
    public class DesignationTBL
    {
        [Key]
        public int designationId { get; set; }

        [Required]
        public int depertmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string designationName { get; set; } = string.Empty;
    }
}
