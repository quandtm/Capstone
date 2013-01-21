#pragma once
#include <SpriteBatch.h>
#include "../resources/ResourceManager.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Graphics
		{
			public enum class OriginPoint
			{
				TopLeft,
				TopRight,
				Center,
				BottomLeft,
				BottomRight
			};

			class TextureData sealed : public Capstone::Engine::Resources::IResource
			{
			public:
				Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> _srv;
				Microsoft::WRL::ComPtr<ID3D11Resource> _resource;
				int _width;
				int _height;

				virtual void Dispose();
				virtual Capstone::Engine::Resources::ResourceStatus Load(const std::wstring& path);
			};

			public ref class Texture sealed : public Capstone::Core::IComponent
			{
			private:
				TextureData *_tex;

				RECT _srcRect;

				Platform::String^ _path;

				volatile bool _isLoaded;
			    volatile bool _loading;

			internal:
				inline void Draw(std::shared_ptr<DirectX::SpriteBatch> batch);
				inline void Load(std::shared_ptr<Capstone::Engine::Resources::ResourceManager> resources);

				property bool IsLoaded
				{
					bool get() { return _isLoaded; }
				}

			public:
				Texture();

				[Capstone::Core::ComponentParameterAttribute(DisplayName="DDS Path")]
				property Platform::String^ Path
				{
					Platform::String^ get() { return _path; }
					void set(Platform::String^ value) { _path = value; }
				}

				virtual property Capstone::Core::Entity^ Entity;

				[Capstone::Core::ComponentParameterAttribute(DisplayName="Source Left")]
				property int SourceRectX
				{
					int get() { return _srcRect.left; }
					void set(int val) { _srcRect.left = val; }
				}
				[Capstone::Core::ComponentParameterAttribute(DisplayName="Source Top")]
				property int SourceRectY
				{
					int get() { return _srcRect.top; }
					void set(int val) { _srcRect.top = val; }
				}
				[Capstone::Core::ComponentParameterAttribute(DisplayName="Source Width")]
				property int SourceRectWidth
				{
					int get() { return _srcRect.right - _srcRect.left; }
					void set(int val) { _srcRect.right = _srcRect.left + val; }
				}
				[Capstone::Core::ComponentParameterAttribute(DisplayName="Source Height")]
				property int SourceRectHeight
				{
					int get() { return _srcRect.bottom - _srcRect.top; }
					void set(int val) { _srcRect.bottom = _srcRect.top + val; }
				}
				
				[Capstone::Core::ComponentParameterAttribute(DisplayName="Visible")]
				property bool IsVisible;
				[Capstone::Core::ComponentParameterAttribute(DisplayName="Origin")]
				property OriginPoint Origin;
			};
		}
	}
}
