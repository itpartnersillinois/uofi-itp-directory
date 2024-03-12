using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using uofi_itp_directory_data.Uploads;

namespace uofi_itp_directory.Controls {

    public partial class DocumentUploader {
        private const string _tempName = "-temp";
        private readonly int _maxAllowedSize = 10000;
        private string _originalDocumentUrl = "";

        public string Cache { get; set; } = DateTime.Now.Ticks.ToString();

        [Parameter]
        public EventCallback Delete { get; set; } = default!;

        public string DocumentResultsText { get; set; } = "";

        public string Filename { get; set; } = "";

        [Parameter]
        public string FileUrl { get; set; } = "";

        [Parameter]
        public bool IsDisabled { get; set; } = false;

        public bool IsTextBoxDisabled => UploaderStatus == UploaderStatusEnum.Uploaded;

        [Parameter]
        public string ItemId { get; set; } = "";

        [Parameter]
        public EventCallback Save { get; set; } = default!;

        public string UploaderId => $"upload";

        public UploaderStatusEnum UploaderStatus { get; set; } = UploaderStatusEnum.Untouched;

        [Inject]
        protected ImageScaler ImageScaler { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        protected UploadStorage UploadStorage { get; set; } = default!;

        public async Task<bool> DeleteFile() {
            if (string.IsNullOrEmpty(_originalDocumentUrl)) {
                _originalDocumentUrl = FileUrl;
            }
            await Delete.InvokeAsync();
            DocumentResultsText = "Document deleted, make sure to save";
            UploaderStatus = UploaderStatusEnum.Deleted;
            return true;
        }

        public async Task<bool> SaveFile() {
            if (UploaderStatus == UploaderStatusEnum.Uploaded) {
                Filename = await UploadStorage.Move(Filename.Replace(_tempName, ""), FileUrl, false);
                FileUrl = UploadStorage.GetFullPath(Filename, false);
                DocumentResultsText = "Document saved";
                return true;
            } else if (UploaderStatus == UploaderStatusEnum.Deleted) {
                _ = await UploadStorage.Delete(_originalDocumentUrl, false);
                DocumentResultsText = "Document deleted";
                return true;
            }
            return false;
        }

        public async Task<bool> SaveFileToTemp(InputFileChangeEventArgs e) {
            if (string.IsNullOrEmpty(_originalDocumentUrl)) {
                _originalDocumentUrl = FileUrl;
            }
            if (e.File.Size > 1024 * _maxAllowedSize) {
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", $"File is too large -- size of file is {float.Round(e.File.Size / (float) (1024 * 1000), 2)}MB and maximum size is {_maxAllowedSize / 1000}MB");
                return false;
            }
            Filename = await UploadStorage.Upload(ItemId + _tempName + Path.GetExtension(e.File.Name), e.File.ContentType, e.File.OpenReadStream(maxAllowedSize: 1024 * _maxAllowedSize), false);
            FileUrl = UploadStorage.GetFullPath(Filename, false);
            Cache = DateTime.Now.Ticks.ToString();
            DocumentResultsText = "New document, make sure to save";
            UploaderStatus = UploaderStatusEnum.Uploaded;
            await Save.InvokeAsync();
            return !string.IsNullOrEmpty(FileUrl);
        }
    }
}