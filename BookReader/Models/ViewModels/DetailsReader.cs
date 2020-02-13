using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookReader.Models.ViewModels
{
    public class DetailsReader
    {
        public Reader Reader { get; set; }
        public List<Book> Books { get; set; }
        public List<Book> notreadBooks { get; set; }
    }
}