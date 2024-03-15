namespace uofi_itp_directory_data.Helpers {

    public static class EnumHelper {

        public static string ToPrettyString(this Enum value) => value.ToString().Replace("_SLASH_", " / ").Replace("__", "-").Replace("_", " ");
    }
}