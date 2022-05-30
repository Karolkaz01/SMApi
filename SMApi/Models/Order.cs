using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SMApi.Models
{

    
    public class Order
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        
        public int UserId { get; set; }

        [JsonIgnore]
        public Desk Desk { get; set; }

        public int DeskId { get; set; }
 
        public DateTime Date { get; set; }

        [Range(1,7)]
        public int NumberOfDays { get; set; }


    }
}
