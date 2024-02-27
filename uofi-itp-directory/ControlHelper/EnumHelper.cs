namespace uofi_itp_directory.ControlHelper {

    public static class EnumHelper {

        public static string ToPrettyString(this Enum value) => value.ToString().Replace("__", "-").Replace("_", " ");
    }
}