using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class EmployeeHour : BaseDataItem {
        public DayOfWeek Day { get; set; }
        public virtual Employee Employee { get; set; } = default!;
        public int EmployeeId { get; set; }
        public string EndTime { get; set; } = "";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public string Notes { get; set; } = "";
        public string StartTime { get; set; } = "";
    }
}