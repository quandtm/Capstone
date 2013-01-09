#pragma once
#include "common\BasicTimer.h"
#include "../graphics/SpriteRenderer.h"

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

				Capstone::Engine::Graphics::SpriteRenderer^ _spriteRenderer;

			internal:
				D3D_FEATURE_LEVEL GetFeatureLevel();

			public:
				Direct3DPanelProxy(void);
				void Initialise(::Windows::UI::Xaml::Controls::SwapChainBackgroundPanel^ panel);
			};
		}
	}
}
