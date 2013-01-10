#include "pch.h"
#include "SpriteRenderer.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Graphics
		{
			SpriteRenderer^ SpriteRenderer::_inst;

			SpriteRenderer::SpriteRenderer()
			{
				_sprites = new TextureList();
				_sprites->reserve(MaxSprites);
				_resAlloc = new Capstone::Engine::Memory::PageAllocator(MaxSprites, sizeof(TextureData));
				_resources = std::make_shared<Capstone::Engine::Resources::ResourceManager>(_resAlloc);
			}

			SpriteRenderer::~SpriteRenderer()
			{
				_device = nullptr;
				_context = nullptr;
				_sb = nullptr;
				delete _sprites;
				_resources = nullptr;
				delete _resAlloc;
			}

			void SpriteRenderer::Init(Microsoft::WRL::ComPtr<ID3D11Device1> device, Microsoft::WRL::ComPtr<ID3D11DeviceContext1> context)
			{
				if (_device == nullptr && _context == nullptr)
				{
					_device = device;
					_context = context;
					_sb = std::make_shared<DirectX::SpriteBatch>(_context.Get());
					_commonStates = std::make_shared<DirectX::CommonStates>(_device.Get());

					for (auto spr : *_sprites)
					{
						if (!spr->IsLoaded)
							spr->Load(_resources);
					}
				}
			}

			void SpriteRenderer::RegisterTexture(Texture^ tex)
			{
				_sprites->push_back(tex);
				if (!tex->IsLoaded && _device != nullptr)
					tex->Load(_resources);
			}

			void SpriteRenderer::RemoveTexture(Texture^ tex)
			{
				TextureIterator it;
				for (auto i = _sprites->begin(); i != _sprites->end(); ++i)
				{
					if ((*i) == tex)
					{
						it = i;
						break;
					}
				}
				_sprites->erase(it);
			}

			void SpriteRenderer::Draw()
			{
				if (_device != nullptr && _context != nullptr)
				{
					_sb->Begin(DirectX::SpriteSortMode_Immediate);
					for (auto t : *_sprites)
						t->Draw(_sb);
					_sb->End();
				}
			}
		}
	}
}
