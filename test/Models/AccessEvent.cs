using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class AccessEvent
    {
        [Key] 
        public int EventId { get; set; }
        public int EmployeeId { get; set; }
        public int RoomId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        public string EventType { get; set; } // "вхід" або "вихід"
        public bool AccessDenied { get; set; } // Перевірка на дозвіл
        public DateTime EventTimestamp { get; set; } // дата та час вхіду або виходу
    }

}
