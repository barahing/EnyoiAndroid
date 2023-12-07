using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnyoiProject.NVVM.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public bool isActive { get; set; }
        public int categoryId { get; set; }
        public Category category { get; set; }
    }
}
