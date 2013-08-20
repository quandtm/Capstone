using SharpDX;
using SharpDX.DXGI;
using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using D3D11 = SharpDX.Direct3D11;

namespace Capstone.Graphics
{
    public sealed class GraphicsDevice
    {
        private SwapChainBackgroundPanel _panel;
        private Stopwatch _sw;

        public SwapChainBackgroundPanel BackgroundPanel
        {
            get { return _panel; }
            set { BindSwapChainBackgroundPanel(value); }
        }

        public bool HasBackgroundPanel { get { return _panel != null; } }

        private D3D11.Device1 _device;
        private D3D11.DeviceContext1 _context;
        private SharpDX.DXGI.SwapChain1 _swap;

        public event Action<double> Tick;

        public GraphicsDevice()
        {
            _panel = null;
            _sw = new Stopwatch();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            _sw.Restart();
            if (Tick != null)
                Tick(_sw.Elapsed.TotalSeconds);
        }

        public void Initialise(int width, int height, params SharpDX.Direct3D.FeatureLevel[] featureLevels)
        {
            // Create device
            var dev = new D3D11.Device(SharpDX.Direct3D.DriverType.Hardware, D3D11.DeviceCreationFlags.None,
                                       featureLevels);
            _device = ComObject.As<D3D11.Device1>(dev.NativePointer);
            _context = _device.ImmediateContext1;

            InitSwapChain(width, height);
        }

        private void InitSwapChain(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("panel must have a valid width or height");

            // Create swap chain for composition and bind to panel
            var desc = new SwapChainDescription1();
            desc.Width = width;
            desc.Height = height;
            desc.Format = Format.B8G8R8A8_UNorm;
            desc.Flags = SwapChainFlags.None;
            desc.BufferCount = 2;
            desc.AlphaMode = AlphaMode.Unspecified;
            desc.Stereo = false;
            desc.Scaling = Scaling.Stretch;
            desc.SampleDescription.Count = 1;
            desc.SampleDescription.Quality = 0;
            desc.SwapEffect = SwapEffect.FlipSequential;
            desc.Usage = Usage.RenderTargetOutput;

            var dxgiDev = ComObject.As<SharpDX.DXGI.Device>(_device.NativePointer);
            var adapter = dxgiDev.Adapter;
            var fact = adapter.GetParent<SharpDX.DXGI.Factory2>();
            _swap = fact.CreateSwapChainForComposition(_device, ref desc, null);
        }

        private void BindSwapChainBackgroundPanel(SwapChainBackgroundPanel panel)
        {
            if (BackgroundPanel != null) throw new Exception("Background panel is already set");
            if (panel == null) throw new ArgumentNullException("panel");
            if (_swap == null) throw new Exception("Device must be initialised first");

            _panel = panel;
            SetChainToPanel();
        }

        private void SetChainToPanel()
        {
            if (!HasBackgroundPanel) return;

            var nativePanel = ComObject.QueryInterface<ISwapChainBackgroundPanelNative>(_panel);
            nativePanel.SwapChain = _swap;
        }

        public void Resize(int width, int height)
        {
            // TODO: Implement resizing
        }
    }
}
