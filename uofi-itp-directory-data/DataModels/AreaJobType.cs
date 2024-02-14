using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class AreaJobType : BaseDataItem {
        public virtual Area Area { get; set; } = default!;

        public int AreaId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public string Title { get; set; } = "";
    }
}