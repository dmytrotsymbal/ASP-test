using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        public virtual ICollection<AccessEvent>? AccessEvents { get; set; }
    }

}



// Ключевое слово virtual в контексте EF используется для
// создания "lazy loading" связей. Это значит, что связанные
// данные (например, AccessEvents для Employee) загружаются только
// тогда, когда они впервые используются в коде, а не при первоначальном
// запросе Employee.
