using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory_data.Cache;
using uofi_itp_directory_data.DataAccess;
using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Helpers;

namespace uofi_itp_directory.Pages.Profile {

    public partial class General {
        private bool _isDirty = false;
        public Dictionary<int, List<AreaTag>> AreaTags { get; set; } = default!;

        public Employee? Employee { get; set; } = default!;

        public string Instructions { get; set; } = "";

        public string PersonName { get; set; } = "My Profile";

        [Parameter]
        public string Refresh { get; set; } = "";

        [SupplyParameterFromQuery(Name = "back")]
        public string? ShowBackButton { get; set; }

        [Inject]
        protected AreaHelper AreaHelper { get; set; } = default!;

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected EmployeeAreaHelper EmployeeAreaHelper { get; set; } = default!;

        [Inject]
        protected EmployeeHelper EmployeeSecurityHelper { get; set; } = default!;

        protected bool IsMultiple => Employee?.JobProfiles.Count() > 1;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        protected int? PrimaryJobProfileId { get; set; }

        public async Task RemoveMessage() => _ = await JsRuntime.InvokeAsync<bool>("removeAlertOnScreen");

        public async Task Send() {
            if (Employee != null) {
                _ = await EmployeeSecurityHelper.SaveEmployee(Employee, await AuthenticationStateProvider.GetUser(), "Employee General");
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Information updated");
                _isDirty = false;
            }
        }

        protected async Task AddTag(AreaTag tag, JobProfile profile) => _ = Employee?.JobProfiles.FirstOrDefault(jp => jp.Id == profile.Id)?.AddTag(tag.Title, tag.AllowEmployeeToEdit) ?? false
                ? await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Tag added")
                : await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Tag already added");

        protected override async Task OnInitializedAsync() {
            var employeeId = CacheHelper.GetCachedEmployee(await AuthenticationStateProvider.GetAuthenticationStateAsync(), CacheHolder, Refresh);
            Employee = await AccessHelper.GetEmployee(await AuthenticationStateProvider.GetAuthenticationStateAsync(), EmployeeSecurityHelper, employeeId);
            if (Employee == null) {
                throw new Exception("No employee");
            }
            AreaTags = [];
            foreach (var areaId in Employee.JobProfiles.Select(jp => jp.Office.AreaId).Distinct()) {
                AreaTags.Add(areaId, await AreaHelper.GetAreaTagsByAreaId(areaId));
            }
            Instructions = await EmployeeAreaHelper.EmployeeInstructions(Employee.NetId);
            PersonName = Employee.ProfileName;
        }

        protected override async Task OnParametersSetAsync() => await OnInitializedAsync();

        protected async Task RemoveTag(JobProfileTag tag, JobProfile profile) {
            if (tag.Id != 0) {
                _ = await EmployeeSecurityHelper.RemoveTag(tag);
            }
            Employee?.JobProfiles.FirstOrDefault(jp => jp.Id == profile.Id)?.Tags.Remove(tag);
        }

        protected void SetDirty() => _isDirty = true;

        private async Task LocationChangingHandler(LocationChangingContext arg) {
            if (_isDirty) {
                if (!(await JsRuntime.InvokeAsync<bool>("confirm", $"You have unsaved changes. Are you sure?"))) {
                    arg.PreventNavigation();
                }
            }
        }
    }
}