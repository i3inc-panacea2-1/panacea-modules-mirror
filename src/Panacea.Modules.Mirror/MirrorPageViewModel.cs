using DirectShowLib;
using Panacea.Core;
using Panacea.Modularity.UiManager;
using Panacea.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Panacea.Modules.Mirror
{
    [View(typeof(MirrorPage))]
    class MirrorPageViewModel:ViewModelBase
    {
        private readonly PanaceaServices _core;
        ILogger _logger;
        public MirrorPageViewModel(PanaceaServices core)
        {
            _core = core;
            _logger = core.Logger;
            try
            {
                //videoCapElement.MediaFailed += (oo, ee) =>
                //{
                //    try
                //    {
                //        _logger.Error(this, "mirror error: " + ee.Exception.Message);

                //        ShowError(ee.Exception.Message);
                //    }
                //    catch
                //    {
                //    }
                //};
            }
            catch
            {
            }
        }

        DsDevice _device;
        public DsDevice Device
        {
            get => _device;
            set
            {
                _device = value;
                OnPropertyChanged();
            }
        }

        void ShowError(string msg)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_core.TryGetUiManager(out IUiManager ui))
                {
                    ui.Toast(msg);
                }
            });
        }

        public override void Activate()
        {
            try
            {
                //even though AMKSVideo contains only the web camera, by selecting it it does not work. we need to get a device from
                //video input category and match it with the name of the device in AMKSVideo. lol.
                var allVideoDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
                var allCameras = DsDevice.GetDevicesOfCat(FilterCategory.AMKSVideo);
                _logger.Debug(this, "VideoInputDevice");
                foreach (var dev in allVideoDevices)
                {
                    _logger.Debug(this, "'" + dev.Name + "'");
                }
                _logger.Debug(this, "=========");
                _logger.Debug(this, "AMKSVideo");
                foreach (var dev in allCameras)
                {
                    _logger.Debug(this, "'" + dev.Name + "'");
                }
                _logger.Debug(this, "=========");
                if (allVideoDevices.Length == 0)
                {
                    ShowError("No video device found");
                    return;
                }
                DsDevice selectedDevice = null;
                if (!string.IsNullOrEmpty("" /*todo Host.MirrorDevice*/))
                {
                    foreach (var ds in allVideoDevices)
                    {
                        _logger.Debug(this, "Mirror device: " + ds.Name);
                        if ("" /*todo Host.MirrorDevice*/.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Contains(ds.Name)) selectedDevice = ds;
                    }
                }
                if (allCameras.Length > 0 && selectedDevice == null)
                {
                    selectedDevice =
                        allVideoDevices.FirstOrDefault(d => allCameras.Any(c => c.Name == d.Name));
                }
                if (selectedDevice == null) return;
                _logger.Debug(this, "host device: " + "" /*todo Host.MirrorDevice*/);



                Device = selectedDevice;
               
                _logger.Debug(this, "Mirror device: " + selectedDevice.Name);
            }
            catch (Exception)
            {
                ShowError("Could not open video device");
            }
        }

        public override void Deactivate()
        {
           Device = null;
        }
    }
}
