using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class DirectoryEntry(int employeeId) : BaseDataItem {
        public DateTime? DateRun { get; set; }

        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        public int EmployeeId { get; set; } = employeeId;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = "";

        public string NetId { get; set; } = "";

        [NotMapped]
        public string Summary => $"{NetId}{(IsSuccessful ? "" : " Failed")}";
    }
}