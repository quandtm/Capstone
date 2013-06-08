#pragma once
#include <SpriteBatch.h>
#include "graphics\Texture.h"
#include "resources\ResourceManager.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Graphics
		{
			public ref class TileRenderer sealed
			{
			private:
				std::shared_ptr<DirectX::SpriteBatch> _sb;
				Microsoft::WRL::ComPtr<ID3D11Device1> _device;
				std::shared_ptr<Capstone::Engine::Resources::ResourceManager> _manager;

				~TileRenderer();

			internal:
				void Draw();
				void Init(Microsoft::WRL::ComPtr<ID3D11Device1> device, Microsoft::WRL::ComPtr<ID3D11DeviceContext1> context);

			public:
				TileRenderer();
			};
		}
	}
}
