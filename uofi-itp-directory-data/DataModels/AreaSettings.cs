using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class AreaSettings : BaseDataItem {
        public bool AllowAdministratorsAccessToPeople { get; set; } = true;
        public bool AllowBeta { get; set; } = false;

        public bool AllowInformationForIllinoisExpertsMembers { get; set; } = false;
        public bool AllowPeople { get; set; } = false;
        public virtual Area Area { get; set; } = default!;

        public int AreaId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public string InstructionsCv { get; set; } = "";
        public string InstructionsEmployee { get; set; } = "";
        public string InstructionsEmployeeActivities { get; set; } = "";
        public string InstructionsEmployeeTags { get; set; } = "";
        public string InstructionsHeadshot { get; set; } = "";
        public string InstructionsMain { get; set; } = "";
        public string InstructionsOfficeTags { get; set; } = "";
        public string InstructionsProfile { get; set; } = "";
        public string InstructionsRefresh { get; set; } = "";
        public string InternalCode { get; set; } = "";
        public string InternalNotes { get; set; } = "";
        public int InternalOrder { get; set; }
        public int PictureHeight { get; set; }
        public int PictureWidth { get; set; }
        public string SignatureExtension { get; set; } = "";
        public string UrlPeopleRefresh { get; set; } = "";
        public string UrlProfile { get; set; } = "";
    }
}