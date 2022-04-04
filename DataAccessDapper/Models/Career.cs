using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessDapper.Models
{
    public class Career
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public IList<CareerItem> CareerItems { get; set; }

        public Career()
        {
            CareerItems = new List<CareerItem>();
        }

    }
}