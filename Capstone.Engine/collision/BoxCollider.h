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
				[Capstone::Core::ComponentParameterAttribute(DisplayName="X")]
				property float X;
				[Capstone::Core::ComponentParameterAttribute(DisplayName="Y")]
				property float Y;
				[Capstone::Core::ComponentParameterAttribute(DisplayName="Width")]
				property float Width;
				[Capstone::Core::ComponentParameterAttribute(DisplayName="Height")]
				property float Height;

				virtual property Entity^ Entity;
				virtual property Platform::String^ Name;

				virtual void Install();
				virtual void Uninstall();
			};
		}
	}
}
