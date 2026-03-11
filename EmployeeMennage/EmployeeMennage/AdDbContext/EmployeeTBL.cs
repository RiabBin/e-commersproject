using System.ComponentModel.DataAnnotations;

namespace EmployeeMennage.AdDbContext
{
    public class EmployeeTBL
    {

        [Key]
        public int employeeId { get; set; }

        [Required]
        public string employeeName { get; set; } = string.Empty;

        [Required]
        public string emContact { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string emMail { get; set; } = string.Empty;

        public string emDistrict { get; set; } = string.Empty;

        public string address { get; set; } = string.Empty;

        public int designationId { get; set; }

        public DateTime createDate { get; set; } = DateTime.UtcNow;
        public string role { get; set; } = string.Empty;
    }
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string emMail { get; set; } = string.Empty;

        [Required]
        public string emContact { get; set; } = string.Empty;
    }



}

