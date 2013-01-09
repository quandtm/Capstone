#include "pch.h"
#include "Texture.h"
#include <DDSTextureLoader.h>
#include <common\DirectXHelper.h>
#include "SpriteRenderer.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Graphics
		{
			// TextureData native type methods
			bool TextureData::Load(const std::wstring& path)
			{
				auto hr = DirectX::CreateDDSTextureFromFile(SpriteRenderer::Instance->Device.Get(), path.data(), &_resource, &_srv);
				_isReady = SUCCEEDED(hr);
				if (_isReady)
				{
					// Get information about the texture
					Microsoft::WRL::ComPtr<ID3D11Texture2D> tex;
					_resource.As(&tex);
					D3D11_TEXTURE2D_DESC desc;
					tex->GetDesc(&desc);
					_width = desc.Width;
					_height = desc.Height;
					return true;
				}
				return false;
			}

			void TextureData::Dispose()
			{
				_srv = nullptr;
				_resource = nullptr;
			}


			// Texture WinRT type methods
			Texture::Texture(Platform::String^ path)
			{
				_path = path;
				_isLoaded = false;
				_loading = false;
				_srcRect = RECT();
				_origin = DirectX::XMFLOAT2(0, 0);
				SpriteRenderer::Instance->RegisterTexture(this);
			}

			void Texture::Load(std::shared_ptr<Capstone::Engine::Resources::ResourceManager> resources)
			{
				if (_loading) return;
				_loading = true;
				resources->LoadAsync<TextureData>(std::wstring(_path->Data()), &_tex).then([this] (bool result)
				{
					if (result)
					{
						this->_srcRect.right = _tex->_width;
						this->_srcRect.bottom = _tex->_height;
					}
					_isLoaded = result;
					_loading = false;
				});
			}

			void Texture::Draw(std::shared_ptr<DirectX::SpriteBatch> batch)
			{
				if (_isLoaded)
				{
					DirectX::XMFLOAT2 pos(Entity->TranslationX, Entity->TranslationY);
					batch->Draw(_tex->_srv.Get(), pos, &_srcRect, DirectX::Colors::White, Entity->Rotation, _origin, Entity->Scale);
				}
			}
		}
	}
}
