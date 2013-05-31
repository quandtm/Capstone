#include "pch.h"
#include "TileRenderer.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Graphics
		{
			TileRenderer::TileRenderer()
			{
			}

			TileRenderer::~TileRenderer()
			{
				_sb = nullptr;
				_device = nullptr;
			}

			void TileRenderer::Init(Microsoft::WRL::ComPtr<ID3D11Device1> device, Microsoft::WRL::ComPtr<ID3D11DeviceContext1> context)
			{
				_sb = std::make_shared<DirectX::SpriteBatch>(context.Get());
				_device = device;
			}

			void TileRenderer::Draw()
			{
			}
		}
	}
}