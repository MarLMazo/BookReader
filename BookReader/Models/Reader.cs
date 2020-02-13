using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReader.Models
{
    public class Reader
    {
        [Key]
        public int ReaderId { get; set; }
       
        [Required(ErrorMessage = "Reader First name Required!")]
        public string ReaderFname { get; set; }
        [Required]
        public string ReaderLname { get; set; }
        [Required]
        public string ReaderAddress { get; set; }
        [Required]
        public int ReaderPhone { get; set; }
    

        public ICollection<Book> Books { get; set; }
    }
}