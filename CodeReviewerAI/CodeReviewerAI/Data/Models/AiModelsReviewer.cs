using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Data.Models
{
    public class AiModelsReviewer
    {
        public int Id { get; set; }
        public string modelName { get; set; }
        public string provider { get; set; }
        public double temperature { get; set; }
    }
}
