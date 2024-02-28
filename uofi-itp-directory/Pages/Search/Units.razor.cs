using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory_data.Cache;
using uofi_itp_directory_data.DataAccess;
using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Helpers;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory.Pages.Search {

    public partial class Units {
        public Area? Area { get; set; }

        public bool IsEditDisabled => LookupId == null;

        public int? LookupId { get; set; }

        public List<LookupThinObject> LookupThinObjects { get; set; } = [];

        [Inject]
        protected AreaHelper AreaHelper { get; set; } = default!;

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected LookupHelper LookupHelper { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected PersonOptionHelper PersonOptionHelper { get; set; } = default!;

        public void ClearArea() => Area = null;

        public async Task EditUnit() {
            if (LookupId != null) {
                Area = await AreaHelper.GetAreaById(LookupId.Value, await AuthenticationStateProvider.GetUser());
                if (await PersonOptionHelper.IsAreaAdmin(await AuthenticationStateProvider.GetUser(), Area.Id)) {
                    var title = LookupThinObjects.First(lto => lto.Id == LookupId.Value).Text;
                    CacheHelper.SetCachedArea(await AuthenticationStateProvider.GetAuthenticationStateAsync(), CacheHolder, new AreaOfficeThinObject(LookupId.Value, title));
                    NavigationManager.NavigateTo("/unit/general");
                }
            }
        }

        protected void ChangeLookupId(ChangeEventArgs e) => LookupId = int.TryParse(e.Value?.ToString(), out var lookupId) ? lookupId : null;

        protected async Task FilterChange(ChangeEventArgs e) {
            LookupThinObjects = await LookupHelper.GetAreas(e.Value?.ToString() ?? "");
        }

        protected override async Task OnInitializedAsync() {
            LookupThinObjects = await LookupHelper.GetAreas("");
        }
    }
}