using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class JobProfile : BaseDataItem {
        public virtual Employee EmployeeProfile { get; set; } = default!;
        public int EmployeeProfileId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public int InternalOrder { get; set; }
        public virtual Office Office { get; set; } = default!;
        public int OfficeId { get; set; }
        public virtual ICollection<JobProfileTag> Tags { get; set; } = default!;
        public string Title { get; set; } = "";
    }
}