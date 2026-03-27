using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.Github
{
    public class GithubOptions
    {
        [Required]
        public string token { get; set; } = string.Empty;
    }
}
