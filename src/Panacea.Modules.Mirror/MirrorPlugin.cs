using Panacea.Core;
using Panacea.Models;
using Panacea.Modularity.Billing;
using Panacea.Modularity.UiManager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Modules.Mirror
{
    public class MirrorPlugin : ICallablePlugin
    {
        private readonly PanaceaServices _core;
        ILogger _logger;

        [PanaceaInject("MirrorWebCamera", "Web camera device name to be used from Mirror plugin. Separate multiple devices with ';'.", "MirrorWebCamera=\"Logitech\"")]
        protected string MirrorWebCamera { get; set; }

        public MirrorPlugin(PanaceaServices core)
        {
            _core = core;
            _logger = core.Logger;
        }
        public async void Call()
        {
            //todo _websocket.PopularNotifyPage("Mirror");
            if (_core.TryGetUiManager(out IUiManager ui))
            {
                if (_core.TryGetBilling(out IBillingManager billing))
                {
                    if (billing.IsPluginFree("Mirror"))
                    {
                        ui.Navigate(new MirrorPageViewModel(_core, MirrorWebCamera));
                    }
                    else
                    {
                        var service = await billing.GetOrRequestServiceAsync("Mirror requires service.", "Mirror");
                        if (service != null)
                        {
                            ui.Navigate(new MirrorPageViewModel(_core, MirrorWebCamera));
                        }
                    }
                }

            }
        }

        public void Dispose()
        {

        }

        public Task BeginInit()
        {
            return Task.CompletedTask;
        }

        public Task EndInit()
        {
            return Task.CompletedTask;
        }
        public Task Shutdown()
        {
            return Task.CompletedTask;
        }

    }
}
