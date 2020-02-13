using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookReader.Models
{
    public class Book
    {
        [Key]

        public int BookId { get; set; }

        [Required(ErrorMessage = "a!!!!")]
        public string BookTitle { get; set; }
        [Required]
        public string BookDescrp { get; set; }
        [Required]
        public string BookAuthor { get; set; }
        [Required]
        public DateTime BookPublish { get; set; }
        

        public ICollection<Reader> Readers { get; set; }
    }
}