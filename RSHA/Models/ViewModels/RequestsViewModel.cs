using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSHA.Models.ViewModels
{
    public class RequestsViewModel
    {
        public Requests Requests { get; set; }
        public IEnumerable<ProblemTypes> ProblemTypes { get; set; }
        public IEnumerable<Mechanics> Mechanics { get; set; }
    }
}
