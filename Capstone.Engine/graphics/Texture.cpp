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
			Texture::Texture() : _path(nullptr), _isLoaded(false), _loading(false), _srcRect(RECT())
			{
				IsVisible = true;
				Origin = OriginPoint::TopLeft;
			}

			void Texture::Setup()
			{
				SpriteRenderer::Instance->RegisterTexture(this);
			}

			void Texture::Load(std::shared_ptr<ResourceManager> resources)
			{
				if (_loading || _path == nullptr) return;
				_loading = true;
				resources->LoadAsync<TextureData>(std::wstring(_path->Data()), &_tex).then([this] (ResourceStatus result)
				{
					if (!(result == ResourceStatus_AllocationError || result == ResourceStatus_LoadingError))
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
				if (_isLoaded && IsVisible)
				{
					DirectX::XMFLOAT2 pos(Entity->Translation->X + OffsetX, Entity->Translation->Y + OffsetY);
					DirectX::XMFLOAT2 origin(0, 0);
					// Calculate the origin, the default is top left so don't handle that
					switch (Origin)
					{
					case OriginPoint::TopRight:
						origin.x = SourceRectWidth;
						break;

					case OriginPoint::Center:
						origin.x = SourceRectWidth / 2.0f;
						origin.y = SourceRectHeight / 2.0f;
						break;

					case OriginPoint::BottomLeft:
						origin.y = SourceRectHeight;
						break;
						
					case OriginPoint::BottomRight:
						origin.x = SourceRectWidth;
						origin.y = SourceRectHeight;
						break;
					}
					batch->Draw(_tex->_srv.Get(), pos, &_srcRect, DirectX::Colors::White, Entity->Rotation, origin, Entity->Scale);
				}
			}
		}
	}
}
