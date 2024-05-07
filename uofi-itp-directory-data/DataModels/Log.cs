using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class Log : BaseDataItem {
        public string ChangedByNetId { get; set; } = "";
        public string ChangeType { get; set; } = "";
        public string Data { get; set; } = "";
        public string DateCreated => LastUpdated.ToString("f");
        public bool EmailSent { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public int SubjectId { get; set; } // employee ID, area ID, office ID
        public string SubjectText { get; set; } = ""; // employee NetID, Area or Office title
        public LogTypeEnum SubjectType { get; set; }
    }
}