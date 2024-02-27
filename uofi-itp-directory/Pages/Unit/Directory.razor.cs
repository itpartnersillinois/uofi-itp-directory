using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory.Controls;
using uofi_itp_directory_data.Cache;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataAccess;
using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory.Pages.Unit {
    public partial class Directory {
        private List<AreaOfficeThinObject> _areaThinObjects = default!;
        private MultiChoice? _multiChoice = default!;
        public Area Area { get; set; } = default!;
        public AreaSettings AreaSettings { get; set; } = default!;
        public string Error { get; set; } = "";
        public string Name { get; set; } = "";
        public string NetId { get; set; } = "";
        public int ProfileInformation { get; set; }
        public int PublishingLocation { get; set; }

        [Parameter]
        public int? UnitId { get; set; }

        public string UnitTitle { get; set; } = "Unit";

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected DirectoryRepository DirectoryRepository { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        protected PersonOptionHelper PersonOptionHelper { get; set; } = default!;

        [Inject]
        protected SecurityEntryHelper SecurityEntryHelper { get; set; } = default!;

        public async Task AssignId() {
            UnitId = _multiChoice?.SelectedId;
            UnitTitle = _multiChoice?.SelectedTitle ?? "";
            await AssignTextFields();
        }

        public async Task Send() {
            _ = await DirectoryRepository.UpdateAsync(AreaSettings);
            _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Directory settings updated");
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync() {
            var cachedAreaThinObject = CacheHelper.GetCachedArea(await AuthenticationStateProvider.GetAuthenticationStateAsync(), CacheHolder);
            if (cachedAreaThinObject != null) {
                UnitId = cachedAreaThinObject.Id;
                UnitTitle = cachedAreaThinObject.Title;
                await AssignTextFields();
            }
            _areaThinObjects = await AccessHelper.GetAreas(await AuthenticationStateProvider.GetAuthenticationStateAsync(), PersonOptionHelper);
            if (_areaThinObjects.IsSingle()) {
                UnitId = _areaThinObjects.Single().Id;
                UnitTitle = _areaThinObjects.Single().Title;
                await AssignTextFields();
            }
        }

        private async Task AssignTextFields() {
            Area = await DirectoryRepository.ReadAsync(d => d.Areas.Single(a => a.Id == UnitId));
            AreaSettings = await DirectoryRepository.ReadAsync(d => d.AreaSettings.Single(a => a.AreaId == UnitId));
        }
    }
}