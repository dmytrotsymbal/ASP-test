using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }  // Список сотрудников в отделе
    }
}
