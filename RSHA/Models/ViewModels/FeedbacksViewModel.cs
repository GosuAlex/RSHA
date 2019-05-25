using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSHA.Models.ViewModels
{
    public class FeedbacksViewModel
    {
        public Feedbacks Feedback { get; set; }
        public IEnumerable<Feedbacks> Feedbacks { get; set; }
    }
}
