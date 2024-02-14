using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class Log : BaseDataItem {
        public string ChangedByNetId { get; set; } = "";
        public string ChangeType { get; set; } = "";
        public string DateCreated => LastUpdated.ToString("f");

        public string Detatils { get; set; } = "";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public string Name { get; set; } = "";
        public string NewData { get; set; } = "";
        public string OldData { get; set; } = "";

        public int SubjectId { get; set; }
        public string SubjectText { get; set; } = "";
        public LogTypeEnum SubjectType { get; set; }
    }
}