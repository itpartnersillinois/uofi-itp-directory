using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class OfficeSettings : BaseDataItem {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public string InternalCode { get; set; } = "";
        public string InternalNotes { get; set; } = "";
        public int InternalOrder { get; set; }
    }
}