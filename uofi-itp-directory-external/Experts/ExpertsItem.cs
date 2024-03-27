namespace uofi_itp_directory_external.Experts {

    public class ExpertsItem {
        public string Institution { get; set; } = "";
        public bool IsHighlighted { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";
        public string Year { get; set; } = "";
        public string YearEnded { get; set; } = "";
    }
}