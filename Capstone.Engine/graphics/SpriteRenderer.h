#pragma once
#include "Texture.h"
#include <vector>
#include "../resources/ResourceManager.h"
#include "../memory/PageAllocator.h"
#include <CommonStates.h>

namespace Capstone
{
	namespace Engine
	{
		namespace Graphics
		{
			typedef std::vector<Capstone::Engine::Graphics::Texture^> TextureList;
			typedef TextureList::iterator TextureIterator;

			public ref class SpriteRenderer sealed
			{
			private:
				static const int MaxSprites = 500;

				Microsoft::WRL::ComPtr<ID3D11Device1> _device;
				Microsoft::WRL::ComPtr<ID3D11DeviceContext1> _context;
				Microsoft::WRL::ComPtr<ID3D11BlendState> _blend;
				std::shared_ptr<DirectX::CommonStates> _commonStates;

				TextureList *_sprites;

				static SpriteRenderer^ _inst;
				SpriteRenderer();
				~SpriteRenderer();
				SpriteRenderer(const SpriteRenderer%);

				std::shared_ptr<DirectX::SpriteBatch> _sb;
				std::shared_ptr<Capstone::Engine::Resources::ResourceManager> _resources;
				Capstone::Engine::Memory::PageAllocator *_resAlloc;

			internal:
				void Init(Microsoft::WRL::ComPtr<ID3D11Device1> device, Microsoft::WRL::ComPtr<ID3D11DeviceContext1> context);
				void Draw();

				property Microsoft::WRL::ComPtr<ID3D11Device1> Device
				{
					Microsoft::WRL::ComPtr<ID3D11Device1> get() { return _device; }
				}
				property Microsoft::WRL::ComPtr<ID3D11DeviceContext1> Context
				{
					Microsoft::WRL::ComPtr<ID3D11DeviceContext1> get() { return _context; }
				}

				void RegisterTexture(Texture^ tex);
				void RemoveTexture(Texture^ tex);

			public:
				static property SpriteRenderer^ Instance
				{
					SpriteRenderer^ get()
					{
						if (_inst == nullptr)
							_inst = ref new SpriteRenderer();
						return _inst;
					}
				}
			};
		}
	}
}
