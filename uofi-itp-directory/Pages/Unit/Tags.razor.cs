using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory.Controls;
using uofi_itp_directory_data.Cache;
using uofi_itp_directory_data.DataAccess;
using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory.Pages.Unit {

    public partial class Tags {
        private List<AreaOfficeThinObject> _areaThinObjects = default!;
        private MultiChoice? _multiChoice = default!;

        public List<AreaTag>? AreaTags { get; set; } = null;

        public bool NewTagEditable { get; set; }

        public string NewTagName { get; set; } = "";

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

        public async Task AssignId() {
            UnitId = _multiChoice?.SelectedId;
            UnitTitle = _multiChoice?.SelectedTitle ?? "";
            await Assign();
        }

        public async Task RemoveMessage() => _ = await JsRuntime.InvokeAsync<bool>("removeAlertOnScreen");

        public async Task RemoveTag(AreaTag tag) {
            if (await JsRuntime.InvokeAsync<bool>("confirm", $"You are removing the tag {tag.Title} from the system. Job profiles will still have the tag and need to be removed independently. Are you really sure you want to do this?")) {
                _ = await AreaHelper.RemoveTag(tag, await AuthenticationStateProvider.GetUser(), UnitTitle);
                AreaTags?.Remove(tag);
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Tag removed");
                StateHasChanged();
            }
        }

        public async Task Send() {
            if (string.IsNullOrWhiteSpace(NewTagName)) {
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Tag name needs to be filled out");
            } else {
                var tag = await AreaHelper.AddTagToArea(UnitId.HasValue ? UnitId.Value : 0, NewTagName, NewTagEditable, await AuthenticationStateProvider.GetUser(), UnitTitle);
                AreaTags?.Add(tag);
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", "Tag added");
                NewTagEditable = false;
                NewTagName = "";
            }
            StateHasChanged();
        }

        protected async Task Assign() {
            AreaTags = await AreaHelper.GetAreaTagsByAreaId(UnitId);
        }

        protected override async Task OnInitializedAsync() {
            var cachedAreaThinObject = CacheHelper.GetCachedArea(await AuthenticationStateProvider.GetAuthenticationStateAsync(), CacheHolder);
            if (cachedAreaThinObject != null) {
                UnitId = cachedAreaThinObject.Id;
                UnitTitle = cachedAreaThinObject.Title;
                await Assign();
            }
            _areaThinObjects = await AccessHelper.GetAreas(await AuthenticationStateProvider.GetAuthenticationStateAsync(), PersonOptionHelper);
            if (_areaThinObjects.IsSingle()) {
                UnitId = _areaThinObjects.Single().Id;
                UnitTitle = _areaThinObjects.Single().Title;
                await Assign();
            }
        }
    }
}