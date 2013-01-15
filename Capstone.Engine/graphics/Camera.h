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
				DirectX::XMMATRIX _view;

			internal:
				Camera(void);

				void UpdateMatrices();

				DirectX::XMMATRIX* GetView() { return &_view; };

			public:
				virtual property Capstone::Core::Entity^ Entity;

				void ScreenToWorld(Capstone::Core::Vector2^ screen, Capstone::Core::Vector2^ world);
			};
		}
	}
}
