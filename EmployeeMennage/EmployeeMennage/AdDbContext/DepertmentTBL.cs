using System.ComponentModel.DataAnnotations;

namespace EmployeeMennage.AdDbContext
{
    public class DepertmentTBL
    {
        [Key]
        public int depertmentId { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "Department name must be between 3 and 100 characters")]
        public string DepertmentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required")]
        public bool IsActive { get; set; }
    }
}
