#pragma once
#include "common\BasicTimer.h"
#include "../graphics/SpriteRenderer.h"
#include "../scripting/ScriptManager.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Windows
		{
			public ref class Direct3DPanelProxy sealed
			{
			private:
				~Direct3DPanelProxy(void);
				Direct3DPanelProxy(const Direct3DPanelProxy% cpy) { };
				Common::BasicTimer^ _timer;
				::Windows::UI::Xaml::Controls::SwapChainBackgroundPanel^ _panel;

				Microsoft::WRL::ComPtr<ID3D11Device1> _device;
				Microsoft::WRL::ComPtr<ID3D11DeviceContext1> _context;
				Microsoft::WRL::ComPtr<ID3D11RenderTargetView> _rtv;
				Microsoft::WRL::ComPtr<IDXGISwapChain1> _swapChain;
				D3D_FEATURE_LEVEL _featureLevel;
				::Windows::Foundation::Rect _windowBounds;

				void CreateDevice();
				void CreateSizeDependentResources();
				void RenderingHandler(Platform::Object^ sender, Platform::Object^ args);
				void Clear();
				void Present();

				void PointerPressedHandler(Platform::Object^ sender, ::Windows::UI::Xaml::Input::PointerRoutedEventArgs ^ args);
				void PointerMovedHandler(Platform::Object^ sender, ::Windows::UI::Xaml::Input::PointerRoutedEventArgs ^ args);
				void PointerReleasedHandler(Platform::Object^ sender, ::Windows::UI::Xaml::Input::PointerRoutedEventArgs ^ args);
				void SizeChangedHandler(::Windows::UI::Core::CoreWindow^ window, ::Windows::UI::Core::WindowSizeChangedEventArgs^ args);

				::Windows::Foundation::EventRegistrationToken _pressedToken, _movedToken, _releasedToken;

				Capstone::Engine::Graphics::SpriteRenderer^ _spriteRenderer;
				Capstone::Engine::Scripting::ScriptManager^ _scriptManager;
				void Initialise();

			internal:
				D3D_FEATURE_LEVEL GetFeatureLevel();

			public:
				Direct3DPanelProxy(void);
				void SetPanel(::Windows::UI::Xaml::Controls::SwapChainBackgroundPanel^ panel);
				property bool IsInitialised
				{
					bool get() { return _swapChain.Get() != nullptr; }
				}
			};
		}
	}
}
