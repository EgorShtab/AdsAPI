using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AwwcoreAPI.Models
{
    public class Ad
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public List<PhotoLink> PhotoLinks { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
    public class PhotoLink
    {
        public int Id { get; set; }
        [Required]
        public string Link { get; set; }
        public int AdId { get; set; }
        public Ad Ad { get; set; }
    }
}
