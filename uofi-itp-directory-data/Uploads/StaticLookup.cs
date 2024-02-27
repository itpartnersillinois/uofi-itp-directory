namespace uofi_itp_directory_data.Uploads {

    public enum UploaderStatusEnum {
        Untouched, Deleted, Uploaded
    }

    public static class StaticLookup {

        public static Dictionary<string, string> SupportedImageTypes = new() {
            {"image/gif", ".gif"},
            {"image/heic", ".heic"},
            {"image/jpeg", ".jpg" },
            {"image/png", ".png"},
            {"image/tiff", ".tif"},
            {"image/webp", ".webp"},
        };
    }
}