using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._ORM.Models
{
    internal class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Pages { get; set; }

        public override string ToString()
        {
            return $"{Id} {Name} {Quantity} {Pages}";
        }
    }
}
