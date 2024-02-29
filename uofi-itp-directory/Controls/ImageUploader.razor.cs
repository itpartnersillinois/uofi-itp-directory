using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using uofi_itp_directory_data.Uploads;

namespace uofi_itp_directory.Controls {

    public partial class ImageUploader {
        public string PhotoText = "Existing Photo";
        private const string _tempName = "-temp";

        private string _originalImageUrl = "";

        public string Cache { get; set; } = DateTime.Now.Ticks.ToString();

        [Parameter]
        public EventCallback Delete { get; set; } = default!;

        public string Filename { get; set; } = "";

        [Parameter]
        public string FileUrl { get; set; } = "";

        [Parameter]
        public int Height { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; } = false;

        public bool IsTextBoxDisabled => UploaderStatus == UploaderStatusEnum.Uploaded;

        [Parameter]
        public string ItemId { get; set; } = "";

        [Parameter]
        public EventCallback Save { get; set; } = default!;

        [Parameter]
        public string Size { get; set; } = "";

        public string UploaderId => $"upload";

        public UploaderStatusEnum UploaderStatus { get; set; } = UploaderStatusEnum.Untouched;

        [Parameter]
        public int Width { get; set; }

        [Inject]
        protected ImageScaler ImageScaler { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        protected UploadStorage UploadStorage { get; set; } = default!;

        public async Task<bool> DeleteFile() {
            if (string.IsNullOrEmpty(_originalImageUrl)) {
                _originalImageUrl = FileUrl;
            }
            await Delete.InvokeAsync();
            UploaderStatus = UploaderStatusEnum.Deleted;
            return true;
        }

        public async Task<bool> SaveFile() {
            if (UploaderStatus == UploaderStatusEnum.Uploaded) {
                Filename = await UploadStorage.Move(Filename.Replace(_tempName, ""), FileUrl, true);
                FileUrl = UploadStorage.GetFullPath(Filename, true);
                return true;
            } else if (UploaderStatus == UploaderStatusEnum.Deleted) {
                _ = await UploadStorage.Delete(_originalImageUrl, true);
                return true;
            }
            return false;
        }

        public async Task<bool> SaveFileToTemp(InputFileChangeEventArgs e) {
            if (string.IsNullOrEmpty(_originalImageUrl)) {
                _originalImageUrl = FileUrl;
            }
            if (Width > 0 && Height > 0) {
                var (newStream, message) = await ImageScaler.Scale(e.File.OpenReadStream(maxAllowedSize: 1024 * 2000), Height, Width, e.File.ContentType);
                if (newStream != null) {
                    newStream.Position = 0;
                    Filename = await UploadStorage.Upload(ItemId + _tempName, "image/webp", newStream, true);
                    FileUrl = UploadStorage.GetFullPath(Filename, true);
                }
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", message);
            } else {
                Filename = await UploadStorage.Upload(ItemId + _tempName, e.File.ContentType, e.File.OpenReadStream(maxAllowedSize: 1024 * 2000), true);
                FileUrl = UploadStorage.GetFullPath(Filename, true);
            }
            PhotoText = "New Photo - not saved yet";
            Cache = DateTime.Now.Ticks.ToString();
            UploaderStatus = UploaderStatusEnum.Uploaded;
            await Save.InvokeAsync();
            return !string.IsNullOrEmpty(FileUrl);
        }
    }
}