using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SMApi.Models
{


    public class Desk
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        public bool IsAvailable { get; set; }

        [JsonIgnore]
        public Location Location { get; set; }

        
        public int LocationId { get; set; }
       
    }
}
