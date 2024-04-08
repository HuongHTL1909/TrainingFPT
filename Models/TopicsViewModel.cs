using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TrainingFPT.Validations;

namespace TrainingFPT.Models
{
    public class TopicsViewModel
    {
        public List<TopicDetail> TopicDetailList { get; set; }
    }
    public class TopicDetail
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Choose Course, please")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Enter name's topic, please")]
        public string NameTopic { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Choose File video, please")]
        [AllowExtensionFile(new string[] { ".avi", ".mp4", ".wmv", ".mkv", ".mov" })]
        [AllowMaxSizeFile(100 * 1024 * 1024)]
        public IFormFile Video { get; set; }
        [AllowNull]
        public string? NameVideo { get; set; }

        [Required(ErrorMessage = "Choose File audio, please")]
        [AllowExtensionFile(new string[] { ".mp3", ".wav", ".ogg", ".flac", ".aac" })]
        [AllowMaxSizeFile(100 * 1024 * 1024)]
        public IFormFile Audio { get; set; }
        [AllowNull]
        public string? NameAudio { get; set; }

        [Required(ErrorMessage = "Choose File document, please")]
        [AllowExtensionFile(new string[] { ".doc", ".docx", ".pdf", ".txt", ".rtf" })]
        [AllowMaxSizeFile(50 * 1024 * 1024)]
        public IFormFile DocumentTopic { get; set; }
        [AllowNull]
        public string? NameDocumentTopic { get; set; }

        public int? LikeTopic { get; set; }
        public int? StartTopic { get; set; }

        [Required(ErrorMessage = "Choose Status, please")]
        public string Status { get; set; }

        public string? viewCourseName { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
