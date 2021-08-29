using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebbShop.Models
{
    public class ImageFile
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public string Filetype { get; set; }
        public byte[] DataFiles { get; set; }
        public Product product { get; set; }
    }
}
