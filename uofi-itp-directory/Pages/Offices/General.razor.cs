﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory.Controls;
using uofi_itp_directory_data.Cache;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataAccess;
using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Helpers;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory.Pages.Offices {

    public partial class General {
        private MultiChoice? _multiChoice = default!;
        private List<AreaOfficeThinObject> _officeThinObjects = default!;
        public string Instructions { get; set; } = "";
        public Office Office { get; set; } = default!;

        [Parameter]
        public int? OfficeId { get; set; }

        public string OfficeTitle { get; set; } = "Office";

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected DirectoryRepository DirectoryRepository { get; set; } = default!;

        [Inject]
        protected EmployeeAreaHelper EmployeeAreaHelper { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        protected PersonOptionHelper PersonOptionHelper { get; set; } = default!;

        [Inject]
        protected SecurityEntryHelper SecurityEntryHelper { get; set; } = default!;

        public async Task AssignId() {
            OfficeId = _multiChoice?.SelectedId;
            OfficeTitle = _multiChoice?.SelectedTitle ?? "";
            await AssignTextFields();
        }

        public async Task Send() {
            _ = DirectoryRepository.Update(Office);
            _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Information updated");
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync() {
            var cachedAreaThinObject = CacheHelper.GetCachedOffice(await AuthenticationStateProvider.GetAuthenticationStateAsync(), CacheHolder);
            if (cachedAreaThinObject != null) {
                OfficeId = cachedAreaThinObject.Id;
                OfficeTitle = cachedAreaThinObject.Title;
                await AssignTextFields();
            }
            _officeThinObjects = await AccessHelper.GetOffices(await AuthenticationStateProvider.GetAuthenticationStateAsync(), PersonOptionHelper);
            if (_officeThinObjects.IsSingle()) {
                OfficeId = _officeThinObjects.Single().Id;
                OfficeTitle = _officeThinObjects.Single().Title;
                await AssignTextFields();
            }
        }

        private async Task AssignTextFields() {
            Office = await DirectoryRepository.ReadAsync(d => d.Offices.Single(a => a.Id == OfficeId));
            Instructions = await EmployeeAreaHelper.OfficeInstructions(OfficeId);
        }
    }
}