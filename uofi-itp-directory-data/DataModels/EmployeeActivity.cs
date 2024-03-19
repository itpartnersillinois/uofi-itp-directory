using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class EmployeeActivity : BaseDataItem {
        public virtual Employee Employee { get; set; } = default!;

        public int EmployeeId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [NotMapped]
        public bool InEditState { get; set; } = false;

        public int InternalOrder { get; set; }
        public string Title { get; set; } = "";
        public ActivityTypeEnum Type { get; set; }
        public string Url { get; set; } = "";
        public string YearEnded { get; set; } = "";
        public string YearStarted { get; set; } = "";
    }
}