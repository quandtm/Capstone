#pragma once

namespace Capstone
{
	namespace Engine
	{
		namespace Collision
		{
			using namespace Capstone::Core;

			public ref class BoxCollider sealed : public Capstone::Core::IComponent
			{
			public:
				virtual property Entity^ Entity;
				virtual property Platform::String^ Name;

				virtual void Install();
				virtual void Uninstall();
			};
		}
	}
}
