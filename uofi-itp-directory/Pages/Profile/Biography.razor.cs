using Blazored.TextEditor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory_data.Cache;
using uofi_itp_directory_data.CampusService;
using uofi_itp_directory_data.DataAccess;
using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Helpers;

namespace uofi_itp_directory.Pages.Profile {

    public partial class Biography {
        private BlazoredTextEditor? QuillBiography = default!;

        public string BiographyText { get; set; } = "";
        public Employee? Employee { get; set; } = default!;

        public bool HideQuillInformationForUpdates { get; set; } = false;

        [Parameter]
        public string Refresh { get; set; } = "";

        public bool ShouldUseExperts { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected EmployeeAreaHelper EmployeeAreaHelper { get; set; } = default!;

        [Inject]
        protected EmployeeHelper EmployeeSecurityHelper { get; set; } = default!;

        [Inject]
        protected IllinoisExpertsManager IllinoisExpertsManager { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        public async Task RemoveMessage() => _ = await JsRuntime.InvokeAsync<bool>("removeAlertOnScreen");

        public async Task Send() {
            if (Employee != null && QuillBiography != null) {
                Employee.Biography = await QuillBiography.GetHTML();
                _ = await EmployeeSecurityHelper.SaveEmployee(Employee, await AuthenticationStateProvider.GetUser(), "Biography: " + Employee.Biography);
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Information updated");
            }
        }

        protected override async Task OnInitializedAsync() {
            var employeeId = CacheHelper.GetCachedEmployee(await AuthenticationStateProvider.GetAuthenticationStateAsync(), CacheHolder, Refresh);
            Employee = await AccessHelper.GetEmployee(await AuthenticationStateProvider.GetAuthenticationStateAsync(), EmployeeSecurityHelper, employeeId);
            if (Employee == null) {
                throw new Exception("No employee");
            } else {
                HideQuillInformationForUpdates = true;
                BiographyText = Employee.Biography;
                HideQuillInformationForUpdates = false;
                ShouldUseExperts = await EmployeeAreaHelper.ShouldUseExperts(Employee.NetId) && await IllinoisExpertsManager.IsInExperts(Employee.NetIdTruncated);
            }
        }

        protected override async Task OnParametersSetAsync() => await OnInitializedAsync();
    }
}