using System.ComponentModel.DataAnnotations;


namespace CodeReviewerAI.Data.Models
{
    public class FileReview
    {
        [Key]
        public int Id { get; set; }


        public int ReviewRecordId { get; set; }
        public virtual ReviewRecord ReviewRecord { get; set; }

        public string FileName { get; set; }
        public string Language { get; set; }
        public string Patch { get; set; }
        public string Commentary { get; set; }
    }
}
