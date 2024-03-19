using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class EmployeeHour : BaseDataItem {

        public static readonly Dictionary<string, LocationTypeEnum> LocationMapping = new() {
            { "In Office", LocationTypeEnum.Office},
            { "", LocationTypeEnum.None },
            { "Remote", LocationTypeEnum.Remote }
        };

        public DayOfWeek Day { get; set; }

        [NotMapped]
        public string DayNumeric => ((int) Day).ToString();

        public virtual Employee Employee { get; set; } = default!;
        public int EmployeeId { get; set; }

        [NotMapped]
        public DateTime? End {
            get { return string.IsNullOrWhiteSpace(EndTime) ? null : DateTime.Parse(EndTime); }
            set { EndTime = value.HasValue ? value.Value.ToShortTimeString() : ""; }
        }

        public string EndTime { get; set; } = "";

        [NotMapped]
        public bool HideNotes => string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(EndTime);

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [NotMapped]
        public bool IsInvalid => Start.HasValue && End.HasValue && Start.Value >= End.Value;

        public string Notes { get; set; } = "";

        [NotMapped]
        public LocationTypeEnum NotesEnum => LocationMapping.ContainsKey(Notes) ? LocationMapping[Notes] : LocationTypeEnum.Other;

        [NotMapped]
        public string OutputText => HideNotes ? "" : $"{StartTime}-{EndTime}{(string.IsNullOrWhiteSpace(Notes) ? "" : " " + Notes)}";

        [NotMapped]
        public DateTime? Start {
            get { return string.IsNullOrWhiteSpace(StartTime) ? null : DateTime.Parse(StartTime); }
            set { StartTime = value.HasValue ? value.Value.ToShortTimeString() : ""; }
        }

        public string StartTime { get; set; } = "";

        public void SetNotes(LocationTypeEnum locationType) {
            Notes = LocationMapping.ContainsValue(locationType) ? LocationMapping.First(lm => lm.Value == locationType).Key : "Other";
        }
    }
}