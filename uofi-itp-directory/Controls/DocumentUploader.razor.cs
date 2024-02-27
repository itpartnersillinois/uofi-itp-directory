﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using uofi_itp_directory_data.Uploads;

namespace uofi_itp_directory.Controls {

    public partial class DocumentUploader {
        private const string _tempName = "-temp";

        private string _originalDocumentUrl = "";

        public string Cache { get; set; } = DateTime.Now.Ticks.ToString();

        [Parameter]
        public EventCallback Delete { get; set; } = default!;

        public string DocumentText { get; set; } = "Existing Document";

        public string Filename { get; set; } = "";

        [Parameter]
        public string FileUrl { get; set; } = "";

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
            UploaderStatus = UploaderStatusEnum.Deleted;
            return true;
        }

        public async Task<bool> SaveFile() {
            if (UploaderStatus == UploaderStatusEnum.Uploaded) {
                Filename = await UploadStorage.Move(Filename.Replace(_tempName, ""), FileUrl, false);
                FileUrl = UploadStorage.GetFullPath(Filename, false);
                return true;
            } else if (UploaderStatus == UploaderStatusEnum.Deleted) {
                _ = await UploadStorage.Delete(_originalDocumentUrl, false);
                return true;
            }
            return false;
        }

        public async Task<bool> SaveFileToTemp(InputFileChangeEventArgs e) {
            if (string.IsNullOrEmpty(_originalDocumentUrl)) {
                _originalDocumentUrl = FileUrl;
            }
            Filename = await UploadStorage.Upload(ItemId + _tempName + Path.GetExtension(e.File.Name), e.File.ContentType, e.File.OpenReadStream(maxAllowedSize: 1024 * 10000), false);
            FileUrl = UploadStorage.GetFullPath(Filename, false);
            Cache = DateTime.Now.Ticks.ToString();
            DocumentText = "New document, make sure to save";
            UploaderStatus = UploaderStatusEnum.Uploaded;
            await Save.InvokeAsync();
            return !string.IsNullOrEmpty(FileUrl);
        }
    }
}