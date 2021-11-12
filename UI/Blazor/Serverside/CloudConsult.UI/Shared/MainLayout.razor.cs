using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace CloudConsult.UI.Shared
{
    public class MainLayoutComponent : LayoutComponentBase
    {
        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        protected RadzenBody body;
        protected RadzenSidebar sidebar;


        protected async System.Threading.Tasks.Task SidebarToggleClick(dynamic args)
        {
            await InvokeAsync(() => { sidebar.Toggle(); });

            await InvokeAsync(() => { body.Toggle(); });
        }
    }
}
