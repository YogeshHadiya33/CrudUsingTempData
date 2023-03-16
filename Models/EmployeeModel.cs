using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CrudUsingTempData.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage ="Please specify first name")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="Please specify last name")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Please specify gender")]
        [DisplayName("Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage ="Please specify department")]
        [DisplayName("Department")]
        public string Department { get; set; }
    }
}
    