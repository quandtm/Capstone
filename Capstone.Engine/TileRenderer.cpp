#include "pch.h"
#include "TileRenderer.h"
#include <DDSTextureLoader.h>

using namespace Capstone::Engine::Resources;

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
				_manager = std::shared_ptr<ResourceManager>(new ResourceManager(new Capstone::Engine::Memory::PageAllocator(30, 1024 * 1024))); // 30MB
			}

			void TileRenderer::Draw()
			{
			}

			void TileRenderer::LoadTileSheet(Platform::String^ name, Platform::String^ path, int width)
			{

			}
		}
	}
}