using System.ComponentModel.DataAnnotations;

namespace SMApi.ModelsDto
{
    public class OrderDto
    {

        public int DeskId { get; set; }

        public DateTime Date { get; set; }

        [Range(1,7)]
        public int NumberOfDays { get; set; }

    }
}
