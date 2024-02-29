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

    public partial class Activities {
        public Employee? Employee { get; set; } = default!;

        public string Instructions { get; set; } = "";

        [Parameter]
        public string Refresh { get; set; } = "";

        public bool ShouldUseExperts { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected EmployeeActivityHelper EmployeeActivityHelper { get; set; } = default!;

        [Inject]
        protected EmployeeAreaHelper EmployeeAreaHelper { get; set; } = default!;

        [Inject]
        protected EmployeeHelper EmployeeSecurityHelper { get; set; } = default!;

        [Inject]
        protected IllinoisExpertsManager IllinoisExpertsManager { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        public async Task Delete(EmployeeActivity activity) => _ = await EmployeeActivityHelper.DeleteActivity(activity, Employee?.Id ?? 0, Employee?.NetId ?? "", await AuthenticationStateProvider.GetUser());

        public void New() {
            if (Employee != null) {
                Employee.EmployeeActivities.Add(new());
            }
        }

        public async Task Save(EmployeeActivity activity) => _ = await EmployeeActivityHelper.SaveActivity(activity, Employee?.Id ?? 0, Employee?.NetId ?? "", await AuthenticationStateProvider.GetUser());

        protected override async Task OnInitializedAsync() {
            var employeeId = CacheHelper.GetCachedEmployee(await AuthenticationStateProvider.GetAuthenticationStateAsync(), CacheHolder, Refresh);
            Employee = await AccessHelper.GetEmployee(await AuthenticationStateProvider.GetAuthenticationStateAsync(), EmployeeSecurityHelper, employeeId);
            if (Employee == null) {
                throw new Exception("No employee");
            } else {
                ShouldUseExperts = await EmployeeAreaHelper.ShouldUseExperts(Employee.NetId) && await IllinoisExpertsManager.IsInExperts(Employee.NetIdTruncated);
                Instructions = await EmployeeAreaHelper.ActivitiesInstructions(Employee.NetId);
            }
        }

        protected override async Task OnParametersSetAsync() => await OnInitializedAsync();
    }
}