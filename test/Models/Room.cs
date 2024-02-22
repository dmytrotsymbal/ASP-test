using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomDescription { get; set; }
        public virtual ICollection<AccessEvent> AccessEvents { get; set; } // Список событий доступа
    }

}
