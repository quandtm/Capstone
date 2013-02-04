#pragma once

namespace Capstone
{
	namespace Engine
	{
		namespace Graphics
		{
			public ref class Camera sealed : Capstone::Core::IComponent
			{
			private:
				_declspec(align(16)) DirectX::XMMATRIX _view;

			internal:
				void UpdateMatrices();

				DirectX::XMMATRIX* GetView() { return &_view; };

			public:
				Camera(void);

				virtual property Capstone::Core::Entity^ Entity;
				virtual property Platform::String^ Name;
				virtual void Setup();
				virtual void Destroy();

				void ScreenToWorld(Capstone::Core::Vector2^ screen, Capstone::Core::Vector2^ world);
			};
		}
	}
}
