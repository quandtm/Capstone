#include "pch.h"
#include "Direct3DPanelProxy.h"
#include "common\DirectXHelper.h"
#include <windows.ui.xaml.media.dxinterop.h>
#include "../graphics/CameraManager.h"

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml;
using namespace Windows::Graphics::Display;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Core;
using namespace Capstone::Engine::Common;
using namespace Microsoft::WRL;
using namespace Capstone::Engine::Graphics;

namespace Capstone
{
	namespace Engine
	{
		namespace Windows
		{
			Direct3DPanelProxy::Direct3DPanelProxy(void)
			{
				_timer = ref new BasicTimer();
			}

			Direct3DPanelProxy::~Direct3DPanelProxy(void)
			{
			}

			void Direct3DPanelProxy::Initialise()
			{
				auto window = Window::Current->CoreWindow;

				// Create base resources
				CreateDevice();
				CreateSizeDependentResources();

				// Hook SizeChanged Event
				window->SizeChanged += ref new TypedEventHandler<CoreWindow^, WindowSizeChangedEventArgs^>(this, &Direct3DPanelProxy::SizeChangedHandler);

				// Hook Rendering event
				CompositionTarget::Rendering::add(ref new EventHandler<Object^>(this, &Direct3DPanelProxy::RenderingHandler));

				_timer->Reset();
				_scriptManager = Capstone::Engine::Scripting::ScriptManager::Instance;
			}

			void Direct3DPanelProxy::SizeChangedHandler(CoreWindow^ window, WindowSizeChangedEventArgs^ args)
			{
				_rtv = nullptr;
				CreateSizeDependentResources();
			}

			void Direct3DPanelProxy::BindPointerEvents()
			{
				_pressedToken = _panel->PointerPressed::add(ref new ::Windows::UI::Xaml::Input::PointerEventHandler(this, &Direct3DPanelProxy::PointerPressedHandler));
				_movedToken = _panel->PointerMoved::add(ref new ::Windows::UI::Xaml::Input::PointerEventHandler(this, &Direct3DPanelProxy::PointerMovedHandler));
				_releasedToken = _panel->PointerReleased::add(ref new ::Windows::UI::Xaml::Input::PointerEventHandler(this, &Direct3DPanelProxy::PointerReleasedHandler));
			}

			void Direct3DPanelProxy::PointerPressedHandler(Platform::Object^ sender, ::Windows::UI::Xaml::Input::PointerRoutedEventArgs^ args)
			{
				PressPointer(args, _panel);
			}

			void Direct3DPanelProxy::PointerMovedHandler(Platform::Object^ sender, ::Windows::UI::Xaml::Input::PointerRoutedEventArgs^ args)
			{
				MovePointer(args, _panel);
			}

			void Direct3DPanelProxy::PointerReleasedHandler(Platform::Object^ sender, ::Windows::UI::Xaml::Input::PointerRoutedEventArgs^ args)
			{
				ReleasePointer(args, _panel);
			}

			void Direct3DPanelProxy::PressPointer(::Windows::UI::Xaml::Input::PointerRoutedEventArgs^ args, ::Windows::UI::Xaml::UIElement^ relativeTo)
			{
				auto pt = args->GetCurrentPoint(relativeTo)->Position;
				_scriptManager->PointerPressed(_timer->Delta, _timer->Total, pt.X, pt.Y);
			}

			void Direct3DPanelProxy::MovePointer(::Windows::UI::Xaml::Input::PointerRoutedEventArgs^ args, ::Windows::UI::Xaml::UIElement^ relativeTo)
			{
				auto pt = args->GetCurrentPoint(relativeTo)->Position;
				_scriptManager->PointerMoved(_timer->Delta, _timer->Total, pt.X, pt.Y);
			}

			void Direct3DPanelProxy::ReleasePointer(::Windows::UI::Xaml::Input::PointerRoutedEventArgs^ args, ::Windows::UI::Xaml::UIElement^ relativeTo)
			{
				auto pt = args->GetCurrentPoint(relativeTo)->Position;
				_scriptManager->PointerReleased(_timer->Delta, _timer->Total, pt.X, pt.Y);
			}

			void Direct3DPanelProxy::CreateDevice()
			{
				D3D_FEATURE_LEVEL featureLevels[] =
				{
					D3D_FEATURE_LEVEL_11_1,
					D3D_FEATURE_LEVEL_11_0,
					D3D_FEATURE_LEVEL_10_1,
					D3D_FEATURE_LEVEL_10_0,
					D3D_FEATURE_LEVEL_9_3
				};

				// Create Device + Context
				ComPtr<ID3D11Device> device;
				ComPtr<ID3D11DeviceContext> context;

				DX::ThrowIfFailed(D3D11CreateDevice(nullptr, D3D_DRIVER_TYPE_HARDWARE, nullptr, 0, featureLevels, ARRAYSIZE(featureLevels), D3D11_SDK_VERSION, &device, &_featureLevel, &context));
				DX::ThrowIfFailed(device.As(&_device));
				DX::ThrowIfFailed(context.As(&_context));

				_spriteRenderer = Capstone::Engine::Graphics::SpriteRenderer::Instance;
				_spriteRenderer->Init(_device, _context);
			}

			void Direct3DPanelProxy::CreateSizeDependentResources()
			{
				_windowBounds = Window::Current->Current->Bounds;

				if (_swapChain != nullptr)
				{
					auto hr = _swapChain->ResizeBuffers(2, static_cast<UINT>(_windowBounds.Width), static_cast<UINT>(_windowBounds.Height), DXGI_FORMAT_B8G8R8A8_UNORM, 0);
					DX::ThrowIfFailed(hr);
				}
				else
				{
					DXGI_SWAP_CHAIN_DESC1 desc = {0};
					desc.Width = static_cast<UINT>(_windowBounds.Width);
					desc.Height = static_cast<UINT>(_windowBounds.Height);
					desc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
					desc.Stereo = false;
					desc.SampleDesc.Count = 1; // No MSAA
					desc.SampleDesc.Quality = 0;
					desc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
					desc.BufferCount = 2;
					desc.Scaling = DXGI_SCALING_STRETCH;
					desc.SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL; // Required for Win Store Apps
					desc.Flags = 0;

					ComPtr<IDXGIDevice1> dxgiDevice;
					DX::ThrowIfFailed(_device.As(&dxgiDevice));
					ComPtr<IDXGIAdapter> dxgiAdapter;
					DX::ThrowIfFailed(dxgiDevice->GetAdapter(&dxgiAdapter));
					ComPtr<IDXGIFactory2> dxgiFactory;
					DX::ThrowIfFailed(dxgiAdapter->GetParent(IID_PPV_ARGS(&dxgiFactory)));

					DX::ThrowIfFailed(dxgiFactory->CreateSwapChainForComposition(_device.Get(), &desc, nullptr, &_swapChain));
					ComPtr<ISwapChainBackgroundPanelNative> nativePanel;
					DX::ThrowIfFailed(reinterpret_cast<IUnknown*>(_panel)->QueryInterface(IID_PPV_ARGS(&nativePanel)));
					DX::ThrowIfFailed(nativePanel->SetSwapChain(_swapChain.Get()));

					// For power consumption, no more than 1 frame can be queued at a time
					DX::ThrowIfFailed(dxgiDevice->SetMaximumFrameLatency(1));
				}

				// Prepare Render Target View
				ComPtr<ID3D11Texture2D> backbuffer;
				DX::ThrowIfFailed(_swapChain->GetBuffer(0, IID_PPV_ARGS(&backbuffer)));
				DX::ThrowIfFailed(_device->CreateRenderTargetView(backbuffer.Get(), nullptr, &_rtv));
				ID3D11RenderTargetView *view = _rtv.Get();
				_context->OMSetRenderTargets(1, &view, nullptr);

				CD3D11_VIEWPORT vp(0.0f, 0.0f, _windowBounds.Width, _windowBounds.Height);
				_context->RSSetViewports(1, &vp);
			}

			void Direct3DPanelProxy::SetPanel(::Windows::UI::Xaml::Controls::SwapChainBackgroundPanel^ panel, bool bindEvents)
			{
				if (_panel != nullptr)
				{
					_panel->PointerPressed::remove(_pressedToken);
					_panel->PointerMoved::remove(_movedToken);
					_panel->PointerReleased::remove(_releasedToken);
				}

				_panel = panel;

				if (_swapChain.Get() == nullptr)
					Initialise();
				else
				{
					ComPtr<ISwapChainBackgroundPanelNative> nativePanel;
					DX::ThrowIfFailed(reinterpret_cast<IUnknown*>(_panel)->QueryInterface(IID_PPV_ARGS(&nativePanel)));
					DX::ThrowIfFailed(nativePanel->SetSwapChain(_swapChain.Get()));
				}

				if (bindEvents)
					BindPointerEvents();
			}

			void Direct3DPanelProxy::RenderingHandler(Object^ sender, Object^ args)
			{
				_timer->Update();
				_scriptManager->Update(_timer->Delta, _timer->Total);

				// The RTV needs to be set every frame
				auto view = _rtv.Get();
				_context->OMSetRenderTargets(1, &view, nullptr);

				CameraManager::Instance->Update();
				_scriptManager->PreDrawUpdate(_timer->Delta, _timer->Total);

				Clear();

				_spriteRenderer->Draw();

				// Draw
				Present();
			}

			void Direct3DPanelProxy::Clear()
			{
				const float clearCol[4] =
				{
					0.392f,
					0.584f,
					0.929f,
					1.0f
				};
				_context->ClearRenderTargetView(_rtv.Get(), clearCol);
			}

			void Direct3DPanelProxy::Present()
			{
				// The application may optionally specify "dirty" or "scroll"
				// rects to improve efficiency in certain scenarios.
				DXGI_PRESENT_PARAMETERS parameters = {0};
				parameters.DirtyRectsCount = 0;
				parameters.pDirtyRects = nullptr;
				parameters.pScrollRect = nullptr;
				parameters.pScrollOffset = nullptr;

				// The first argument instructs DXGI to block until VSync, putting the application
				// to sleep until the next VSync. This ensures we don't waste any cycles rendering
				// frames that will never be displayed to the screen.
				HRESULT hr = _swapChain->Present1(1, 0, &parameters);

				// Discard the contents of the render target.
				// This is a valid operation only when the existing contents will be entirely
				// overwritten. If dirty or scroll rects are used, this call should be removed.
				_context->DiscardView(_rtv.Get());

				DX::ThrowIfFailed(hr);
			}

			D3D_FEATURE_LEVEL Direct3DPanelProxy::GetFeatureLevel()
			{
				return _featureLevel;
			}
		}
	}
}
