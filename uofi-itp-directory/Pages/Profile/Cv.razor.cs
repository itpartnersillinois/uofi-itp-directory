using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory.Controls;
using uofi_itp_directory_data.Cache;
using uofi_itp_directory_data.DataAccess;
using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Helpers;

namespace uofi_itp_directory.Pages.Profile {

    public partial class Cv {
        public DocumentUploader? DocumentUploader { get; set; } = default!;

        public Employee? Employee { get; set; } = default!;

        public string Instructions { get; set; } = "";

        [Parameter]
        public string Refresh { get; set; } = "";

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected EmployeeAreaHelper EmployeeAreaHelper { get; set; } = default!;

        [Inject]
        protected EmployeeHelper EmployeeSecurityHelper { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        public void DeleteDocument() {
            if (Employee != null && DocumentUploader != null) {
                Employee.CVUrl = "";
                _isDirty = true;
                StateHasChanged();
            }
        }

        public async Task RemoveMessage() => _ = await JsRuntime.InvokeAsync<bool>("removeAlertOnScreen");

        public void SaveDocument() {
            if (Employee != null && DocumentUploader != null) {
                Employee.CVUrl = DocumentUploader.FileUrl;
                _isDirty = true;
                StateHasChanged();
            }
        }

        public async Task Send() {
            if (Employee != null && DocumentUploader != null) {
                if (await DocumentUploader.SaveFile()) {
                    Employee.CVUrl = DocumentUploader.FileUrl;
                    _ = await EmployeeSecurityHelper.SaveEmployee(Employee, await AuthenticationStateProvider.GetUser(), "CV Updated");
                    _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Information updated");
                    _isDirty = false;
                }
            }
        }

        protected override async Task OnInitializedAsync() {
            var employeeId = CacheHelper.GetCachedEmployee(await AuthenticationStateProvider.GetAuthenticationStateAsync(), CacheHolder, Refresh);
            Employee = await AccessHelper.GetEmployee(await AuthenticationStateProvider.GetAuthenticationStateAsync(), EmployeeSecurityHelper, employeeId);
            if (Employee == null) {
                throw new Exception("No employee");
            }
            Instructions = await EmployeeAreaHelper.CvInstructions(Employee.NetId);
        }

        private bool _isDirty = false;

        private async Task LocationChangingHandler(LocationChangingContext arg) {
            if (_isDirty) {
                if (!(await JsRuntime.InvokeAsync<bool>("confirm", $"You have unsaved changes. Are you sure?"))) {
                    arg.PreventNavigation();
                }
            }
        }

        protected override async Task OnParametersSetAsync() => await OnInitializedAsync();
    }
}