using GR30321.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR30321.Domain.Models
{
    public class CartItem
    {
        public Book Item { get; set; }
        public int Qty { get; set; }

    }
}
