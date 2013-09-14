using System;
using System.Diagnostics;
using SharpDX;
using SharpDX.DXGI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using D3D11 = SharpDX.Direct3D11;

namespace Capstone.Graphics
{
    public sealed class XamlGraphicsDevice : IDisposable
    {
        private SwapChainBackgroundPanel _panel;
        private Stopwatch _sw;

        public SwapChainBackgroundPanel BackgroundPanel
        {
            get { return _panel; }
            set { BindSwapChainBackgroundPanel(value); }
        }
        public bool HasBackgroundPanel { get { return _panel != null; } }
        public bool EnableGameLoop { get; private set; }
        public Color4 ClearColour { get; set; }

        public SharpDX.Toolkit.Graphics.GraphicsDevice ToolkitDevice { get; private set; }

        public D3D11.Device1 Device
        {
            get { return _device; }
        }
        public D3D11.DeviceContext1 Context
        {
            get { return _context; }
        }
        public SwapChain1 SwapChain
        {
            get { return _swap; }
        }
        public D3D11.RenderTargetView BackbufferView
        {
            get { return _rtv; }
        }

        private D3D11.Device1 _device;
        private D3D11.DeviceContext1 _context;
        private SwapChain1 _swap;
        private int _width, _height;
        private D3D11.RenderTargetView _rtv;

        public event Action<double> Update;
        public event Action<XamlGraphicsDevice, double> Draw;

        private static volatile XamlGraphicsDevice _inst = null;
        private static volatile object _sync = new object();
        public static XamlGraphicsDevice Instance
        {
            get
            {
                lock (_sync)
                {
                    if (_inst == null)
                        _inst = new XamlGraphicsDevice();
                }
                return _inst;
            }
        }

        private XamlGraphicsDevice()
        {
            _panel = null;
            _sw = new Stopwatch();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            EnableGameLoop = false;
            ClearColour = Color4.Black;
        }

        ~XamlGraphicsDevice()
        {
            Dispose();
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            _sw.Stop();
            if (EnableGameLoop)
            {

                if (Update != null)
                    Update(_sw.Elapsed.TotalSeconds);

                // Clear
                _context.OutputMerger.SetRenderTargets(_rtv);
                _context.ClearRenderTargetView(_rtv, ClearColour);

                if (Draw != null)
                    Draw(this, _sw.Elapsed.TotalSeconds);

                var pp = new PresentParameters()
                    {
                        DirtyRectangles = null,
                        ScrollOffset = null,
                        ScrollRectangle = null
                    };

                // Present
                _swap.Present(1, PresentFlags.None, pp);
                _context.DiscardView(_rtv);
            }
            _sw.Start();
        }

        public void Initialise(int width, int height, params SharpDX.Direct3D.FeatureLevel[] featureLevels)
        {
            // Create device
            var dev = new D3D11.Device(SharpDX.Direct3D.DriverType.Hardware, D3D11.DeviceCreationFlags.None,
                                       featureLevels);
            _device = ComObject.As<D3D11.Device1>(dev.NativePointer);
            _context = _device.ImmediateContext1;

            ToolkitDevice = SharpDX.Toolkit.Graphics.GraphicsDevice.New(_device);

            InitSwapChain(width, height);
            RetrieveSetBuffers();
            EnableGameLoop = true;
        }

        private void InitSwapChain(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("panel must have a valid width or height");

            // Create swap chain for composition and bind to panel
            var desc = new SwapChainDescription1
                {
                    Width = width,
                    Height = height,
                    Format = Format.B8G8R8A8_UNorm,
                    Flags = SwapChainFlags.None,
                    BufferCount = 2,
                    AlphaMode = AlphaMode.Unspecified,
                    Stereo = false,
                    Scaling = Scaling.Stretch,
                    SampleDescription = { Count = 1, Quality = 0 },
                    SwapEffect = SwapEffect.FlipSequential,
                    Usage = Usage.RenderTargetOutput
                };

            _width = width;
            _height = height;

            var dxgiDev = ComObject.As<SharpDX.DXGI.Device1>(_device.NativePointer);
            var adapter = dxgiDev.Adapter;
            var fact = adapter.GetParent<SharpDX.DXGI.Factory2>();
            _swap = fact.CreateSwapChainForComposition(_device, ref desc, null);

            dxgiDev.MaximumFrameLatency = 1;
        }

        private void BindSwapChainBackgroundPanel(SwapChainBackgroundPanel panel)
        {
            if (BackgroundPanel != null) throw new Exception("Background panel is already set");
            if (panel == null) throw new ArgumentNullException("panel");
            if (_swap == null) throw new Exception("Device must be initialised first");

            _panel = panel;
            SetChainToPanel();
        }

        private void RetrieveSetBuffers()
        {
            if (_swap == null) throw new NullReferenceException("SwapChain is null");

            var bb = _swap.GetBackBuffer<D3D11.Texture2D>(0);
            _rtv = new D3D11.RenderTargetView(_device, bb);
            bb.Dispose();

            _context.OutputMerger.SetRenderTargets(_rtv);
            _context.Rasterizer.SetViewport(0, 0, _width, _height);
        }

        private void SetChainToPanel()
        {
            if (!HasBackgroundPanel) return;

            var nativePanel = ComObject.QueryInterface<ISwapChainBackgroundPanelNative>(_panel);
            nativePanel.SwapChain = _swap;
        }

        public void Resize(int width, int height)
        {
            if (width != _width || height != _height)
            {
                if (_rtv != null)
                {
                    _rtv.Dispose();
                    _rtv = null;
                }

                if (_swap != null)
                {
                    _swap.ResizeBuffers(2, width, height, Format.B8G8R8A8_UNorm, SwapChainFlags.None);
                    _width = width;
                    _height = height;
                }
                else
                {
                    InitSwapChain(width, height);
                    SetChainToPanel();
                }
                RetrieveSetBuffers();
            }
        }

        public void Dispose()
        {
            if (_rtv != null)
                _rtv.Dispose();
            _rtv = null;

            if (_swap != null)
                _swap.Dispose();
            _swap = null;

            if (ToolkitDevice != null)
                ToolkitDevice.Dispose();
            ToolkitDevice = null;

            if (_context != null)
                _context.Dispose();
            _context = null;

            if (_device != null)
                _device.Dispose();
            _device = null;

            _width = 0;
            _height = 0;
        }
    }
}
