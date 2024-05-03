using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class JobProfile : BaseDataItem {
        public ProfileCategoryTypeEnum Category { get; set; }
        public string Description { get; set; } = "";
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
            get => !IsActive ? ProfileDisplayEnum.Not_Displayed : (ProfileDisplayEnum) InternalOrder;
            set {
                IsActive = value != ProfileDisplayEnum.Not_Displayed;
                InternalOrder = (int) value;
            }
        }

        public virtual ICollection<JobProfileTag> Tags { get; set; } = default!;
        public string Title { get; set; } = "";

        public bool AddTag(string title, bool allowEdit) {
            if (Tags != null && !Tags.Any(t => t.Title == title)) {
                Tags.Add(new JobProfileTag { Title = title, AllowEmployeeToEdit = allowEdit, IsActive = true, LastUpdated = DateTime.Now });
                return true;
            }
            return false;
        }
    }
}