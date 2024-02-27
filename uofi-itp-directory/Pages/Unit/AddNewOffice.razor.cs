using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory.Controls;
using uofi_itp_directory_data.Cache;
using uofi_itp_directory_data.CampusService;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataAccess;
using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory.Pages.Unit {

    public partial class AddNewOffice {
        private List<AreaOfficeThinObject> _areaThinObjects = default!;
        private MultiChoice? _multiChoice = default!;
        public string Error { get; set; } = "";
        public string Name { get; set; } = "";
        public string NetId { get; set; } = "";
        public string OfficeName { get; set; } = "";
        public List<Office> Offices { get; set; } = default!;
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
        protected DataWarehouseManager DataWarehouseManager { get; set; } = default!;

        [Inject]
        protected DirectoryRepository DirectoryRepository { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        protected OfficeHelper OfficeHelper { get; set; } = default!;

        [Inject]
        protected PersonOptionHelper PersonOptionHelper { get; set; } = default!;

        [Inject]
        protected SecurityEntryHelper SecurityEntryHelper { get; set; } = default!;

        public async Task AssignId() {
            UnitId = _multiChoice?.SelectedId;
            UnitTitle = _multiChoice?.SelectedTitle ?? "";
            await AssignTextFields();
        }

        public async Task LookupId() {
            var name = await DataWarehouseManager.GetDataWarehouseItem(NetId);
            _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", string.IsNullOrWhiteSpace(name.Name) ? "No name found" : name.Name);
        }

        public async Task RemoveEntry(int id) {
            var numberItemsDeleted = await OfficeHelper.RemoveOffice(Offices.Single(o => o.Id == id), await AuthenticationStateProvider.GetUser());
            if (numberItemsDeleted != 0) {
                Offices.RemoveAll(o => o.Id == id);
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Office deleted");
            } else {
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Offices were in the list");
            }
        }

        public async Task Send() {
            if (!string.IsNullOrWhiteSpace(NetId) && !string.IsNullOrWhiteSpace(OfficeName) && UnitId.HasValue) {
                var message = await OfficeHelper.GenerateOffice(OfficeName, UnitId.Value, NetId, await AuthenticationStateProvider.GetUser());
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", message);
                StateHasChanged();
            } else {
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", "NetID and Office Name are required");
            }
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
            Offices = [.. (await DirectoryRepository.ReadAsync(d => d.Offices.Where(a => a.AreaId == UnitId)))];
        }
    }
}