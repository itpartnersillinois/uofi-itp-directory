using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_external.ImageManager;

namespace uofi_itp_directory.Controllers {

    [Route("[controller]")]
    [AllowAnonymous]
    public class PictureController(DirectoryRepository? directoryRepository) : Controller {
        private readonly DirectoryRepository _directoryRepository = directoryRepository ?? throw new ArgumentNullException("directoryRepository");

        [Route("{netid}")]
        [HttpGet]
        public async Task<IActionResult> ByUsername(string netid) => await Index(netid);

        [HttpGet]
        public async Task<IActionResult> Index(string netid) {
            netid = netid.Replace("@illinois.edu", "") + "@illinois.edu";
            var url = (await _directoryRepository.ReadAsync(d => d.Employees.FirstOrDefault(e => e.NetId == netid)))?.PhotoUrl ?? "";
            if (string.IsNullOrWhiteSpace(url)) {
                url = DirectoryImage.Blank;
            }
            var wc = new HttpClient();
            var stream = await wc.GetStreamAsync(url);
            return new FileStreamResult(stream, "image/webp");
        }
    }
}