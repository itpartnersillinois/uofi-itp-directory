using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class JobProfile : BaseDataItem {
        public ProfileCategoryTypeEnum Category { get; set; }
        public virtual Employee EmployeeProfile { get; set; } = default!;
        public int EmployeeProfileId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public int InternalOrder { get; set; } = 3;

        [NotMapped]
        public bool IsEntryDisabled { get; set; } = true; // used by the code to determine if the user can edit this object

        public virtual Office Office { get; set; } = default!;
        public int OfficeId { get; set; }

        [NotMapped]
        public ProfileDisplayEnum ProfileDisplay {
            get => !IsActive ? ProfileDisplayEnum.Not_Listed : (ProfileDisplayEnum) InternalOrder;
            set {
                IsActive = value != ProfileDisplayEnum.Not_Listed;
                InternalOrder = (int) value;
            }
        }

        public virtual ICollection<JobProfileTag> Tags { get; set; } = default!;
        public string Title { get; set; } = "";
    }
}