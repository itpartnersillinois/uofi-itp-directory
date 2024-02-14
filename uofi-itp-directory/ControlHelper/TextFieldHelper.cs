using uofi_itp_directory.Controls;

namespace uofi_itp_directory.ControlHelper {

    public static class TextFieldHelper {

        public static List<string> GetErrors(this List<LabelAndText> list) => list.Select(l => l.IsError()).Where(ls => !string.IsNullOrWhiteSpace(ls)).ToList();

        public static string GetValue(this List<LabelAndText> list, string label) => list.First(t => t.Label.Contains(label, StringComparison.OrdinalIgnoreCase)).Value.Trim();
    }
}