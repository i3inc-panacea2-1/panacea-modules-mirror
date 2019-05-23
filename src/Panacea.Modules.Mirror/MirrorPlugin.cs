﻿using Panacea.Core;
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
        public MirrorPlugin(PanaceaServices core)
        {
            _core = core;
            _logger = core.Logger;
        }
        public void Call()
        {
            //todo _websocket.PopularNotifyPage("Mirror");
            if (_core.TryGetUiManager(out IUiManager ui))
            {
                ui.Navigate(new MirrorPageViewModel(_core));
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