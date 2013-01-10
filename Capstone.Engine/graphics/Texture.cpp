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
			using namespace Capstone::Engine::Resources;

			// TextureData native type methods
			ResourceStatus TextureData::Load(const std::wstring& path)
			{
				_status = ResourceStatus_Loading;
				auto hr = DirectX::CreateDDSTextureFromFile(SpriteRenderer::Instance->Device.Get(), path.data(), &_resource, &_srv);
				auto result = SUCCEEDED(hr);
				if (result)
				{
					// Get information about the texture
					Microsoft::WRL::ComPtr<ID3D11Texture2D> tex;
					_resource.As(&tex);
					D3D11_TEXTURE2D_DESC desc;
					tex->GetDesc(&desc);
					_width = desc.Width;
					_height = desc.Height;
					_status = ResourceStatus_Loaded;
				}
				else
					_status = ResourceStatus_LoadingError;
				return _status;
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
			}

			void Texture::Load(std::shared_ptr<ResourceManager> resources)
			{
				if (_loading) return;
				_loading = true;
				resources->LoadAsync<TextureData>(std::wstring(_path->Data()), &_tex).then([this] (ResourceStatus result)
				{
					if (result == ResourceStatus_AllocationError || result == ResourceStatus_LoadingError)
					{
						_isLoaded = false;
					}
					else
					{
						if (result == ResourceStatus_Loading)
						{
							while (_tex->GetStatus() == ResourceStatus_Loading) {}
						}

						if (_tex->GetStatus() == ResourceStatus_Loaded)
						{
							_srcRect.right = _tex->_width;
							_srcRect.bottom = _tex->_height;
							_isLoaded = true;
						}
						else
							_isLoaded = false;
					}
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
