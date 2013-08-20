using SharpDX;
using SharpDX.DXGI;
using System;
using Windows.UI.Xaml.Controls;
using D3D11 = SharpDX.Direct3D11;

namespace Capstone.Graphics
{
    public class GraphicsDevice
    {
        public SwapChainBackgroundPanel BackgroundPanel { get; private set; }

        private D3D11.Device1 _device;
        private D3D11.DeviceContext1 _context;
        private SharpDX.DXGI.SwapChain1 _swap;

        public GraphicsDevice()
        {
        }

        public void Initialise(params SharpDX.Direct3D.FeatureLevel[] featureLevels)
        {
            // Create device
            var dev = new D3D11.Device(SharpDX.Direct3D.DriverType.Hardware, D3D11.DeviceCreationFlags.None,
                                       featureLevels);
            _device = ComObject.As<D3D11.Device1>(dev.NativePointer);
            _context = _device.ImmediateContext1;
        }

        public void BindSwapChainBackgroundPanel(SwapChainBackgroundPanel panel)
        {
            if (_device == null) throw new NullReferenceException("Device must be initialised first");
            if (BackgroundPanel != null) throw new Exception("BackgroundPanel already set");
            if (panel == null) throw new ArgumentNullException("panel");
            if (panel.Width <= 0 || panel.Height <= 0)
                throw new ArgumentException("panel must have a valid width or height");

            BackgroundPanel = panel;
            // Create swap chain for composition and bind to panel
            var desc = new SwapChainDescription1();
            desc.Width = (int)panel.Width;
            desc.Height = (int)panel.Height;
            desc.Format = Format.R8G8B8A8_UNorm;
            desc.Flags = SwapChainFlags.None;
            desc.BufferCount = 2;
            desc.AlphaMode = AlphaMode.Premultiplied;
            desc.Stereo = false;
            desc.Scaling = Scaling.None;
            desc.SampleDescription.Count = 1;
            desc.SampleDescription.Quality = 0;
            desc.SwapEffect = SwapEffect.FlipSequential;
            desc.Usage = Usage.RenderTargetOutput;

            var nativePanel = ComObject.QueryInterface<ISwapChainBackgroundPanelNative>(panel);

            var dxgiDev = ComObject.As<SharpDX.DXGI.Device>(_device.NativePointer);
            var adapter = dxgiDev.Adapter;
            var fact = adapter.GetParent<SharpDX.DXGI.Factory2>();
            _swap = fact.CreateSwapChainForComposition(_device, ref desc, null);
            // TODO: Is this correct? ^
        }
    }
}
