using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookReader.Models.ViewModels
{
    public class DetailsBook
    {
        public Book Book { get; set; }
        public List<Reader> Readers { get; set; }
        public List<Reader> NotReaders { get; set; }
    }
}