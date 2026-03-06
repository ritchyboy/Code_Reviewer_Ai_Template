using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Models
{
    public class GithubFileChange
    {
        public string fileHeader { get; set; }
        public string fileName { get; set; }
        public string patch { get; set; }
    }
}
