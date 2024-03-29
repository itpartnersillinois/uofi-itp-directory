﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory.Controls;
using uofi_itp_directory_data.Cache;
using uofi_itp_directory_data.DataAccess;
using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory.Pages.Unit {

    public partial class Internal {
        private List<AreaOfficeThinObject> _areaThinObjects = default!;
        private MultiChoice? _multiChoice = default!;
        public Area Area { get; set; } = default!;
        public AreaSettings AreaSettings { get; set; } = default!;
        public int ProfileInformation { get; set; }
        public int PublishingLocation { get; set; }

        [Parameter]
        public int? UnitId { get; set; }

        public string UnitTitle { get; set; } = "Unit";

        [Inject]
        protected AreaHelper AreaHelper { get; set; } = default!;

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        protected PersonOptionHelper PersonOptionHelper { get; set; } = default!;

        [Inject]
        protected SecurityEntryHelper SecurityEntryHelper { get; set; } = default!;

        private bool _originalAllowAccess { get; set; }

        public async Task AssignId() {
            UnitId = _multiChoice?.SelectedId;
            UnitTitle = _multiChoice?.SelectedTitle ?? "";
            await AssignTextFields();
        }

        public async Task RemoveMessage() => _ = await JsRuntime.InvokeAsync<bool>("removeAlertOnScreen");

        public async Task Send() {
            Area.IsActive = PublishingLocation > 0;
            Area.IsInternalOnly = PublishingLocation == 1;
            AreaSettings.AllowPeople = ProfileInformation > 0;
            AreaSettings.AllowAdministratorsAccessToPeople = ProfileInformation > 1;
            AreaSettings.AllowInformationForIllinoisExpertsMembers = ProfileInformation > 2;
            _ = await AreaHelper.UpdateArea(Area, await AuthenticationStateProvider.GetUser());
            _ = await AreaHelper.UpdateAreaSettings(AreaSettings, Area.Title, await AuthenticationStateProvider.GetUser());
            if (_originalAllowAccess != AreaSettings.AllowAdministratorsAccessToPeople) {
                _ = await SecurityEntryHelper.SetAccessToOtherPeople(Area.Id, AreaSettings.AllowAdministratorsAccessToPeople);
                _originalAllowAccess = AreaSettings.AllowAdministratorsAccessToPeople;
            }
            // TODO if someone changes a profile URL, do we need to update this to all employees in the area?
            _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Internal settings updated");
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

        private static int SetProfileInformation(AreaSettings areaSettings) {
            if (!areaSettings.AllowPeople) {
                return 0;
            }
            if (!areaSettings.AllowAdministratorsAccessToPeople) {
                return 1;
            }
            if (!areaSettings.AllowInformationForIllinoisExpertsMembers) {
                return 2;
            }
            return 3;
        }

        private static int SetPublishingLocation(Area area) {
            if (!area.IsActive) {
                return 0;
            }
            if (area.IsInternalOnly) {
                return 1;
            }
            return 2;
        }

        private async Task AssignTextFields() {
            Area = await AreaHelper.GetAreaById(UnitId, await AuthenticationStateProvider.GetUser());
            AreaSettings = await AreaHelper.GetAreaSettingsByAreaId(UnitId);
            PublishingLocation = SetPublishingLocation(Area);
            ProfileInformation = SetProfileInformation(AreaSettings);
            _originalAllowAccess = AreaSettings.AllowAdministratorsAccessToPeople;
        }
    }
}