#pragma once

#include "DirectXHelper.h"

// Helper class that initializes DirectX APIs for 3D rendering.
class Direct3DBase
{
public:
	Direct3DBase();
	virtual void Initialize(Windows::UI::Core::CoreWindow^ window);
	virtual void HandleDeviceLost();
	virtual void CreateDeviceResources();
	virtual void CreateWindowSizeDependentResources();
	virtual void UpdateForWindowSizeChange();
	virtual void Present();
	virtual float ConvertDipsToPixels(float dips);
	void Clear();

	Microsoft::WRL::ComPtr<ID3D11DeviceContext1> GetDeviceContext(void) { return m_d3dContext; }
	Microsoft::WRL::ComPtr<ID3D11Device1> GetDevice(void) { return m_d3dDevice; }
	Microsoft::WRL::ComPtr<ID3D11RenderTargetView> GetRTV(void) { return m_renderTargetView; }

private:
	// Direct3D Objects.
	Microsoft::WRL::ComPtr<ID3D11Device1> m_d3dDevice;
	Microsoft::WRL::ComPtr<ID3D11DeviceContext1> m_d3dContext;
	Microsoft::WRL::ComPtr<IDXGISwapChain1> m_swapChain;
	Microsoft::WRL::ComPtr<ID3D11RenderTargetView> m_renderTargetView;
	Microsoft::WRL::ComPtr<ID3D11DepthStencilView> m_depthStencilView;

	// Cached renderer properties.
	D3D_FEATURE_LEVEL m_featureLevel;
	Windows::Foundation::Size m_renderTargetSize;
	Windows::Foundation::Rect m_windowBounds;
	Platform::Agile<Windows::UI::Core::CoreWindow> m_window;
	Windows::Graphics::Display::DisplayOrientations m_orientation;

	// Transform used for display orientation.
	DirectX::XMFLOAT4X4 m_orientationTransform3D;
};
