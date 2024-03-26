namespace uofi_itp_directory_external.ImageManager {

    public static class DirectoryImage {
        private const string _blank = "https://directory.illinois.edu/webservices/public/ds/profile.png";

        public static string CheckImage(string url) {
            if (string.IsNullOrWhiteSpace(url)) {
                return _blank;
            }
            using var client = new HttpClient();
            using var res = client.GetAsync(url).Result;
            return res.IsSuccessStatusCode ? url : _blank;
        }

        public static string GetCampusImagePathFromNetId(string netid) => CheckImage($"https://directory.illinois.edu/staff/{netid}@illinois.edu/profile.png");
    }
}