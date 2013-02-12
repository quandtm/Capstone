#pragma once

namespace Capstone
{
	namespace Engine
	{
		namespace Collision
		{
			using namespace Capstone::Core;

			public ref class DistanceCollider sealed : public IComponent
			{
			public:
				[ComponentParameterAttribute(DisplayName="Collision Distance")]
				property float Distance;
				[ComponentParameterAttribute(DisplayName="X Offset")]
				property float OffsetX;
				[ComponentParameterAttribute(DisplayName="Y Offset")]
				property float OffsetY;

				virtual property Entity^ Entity;
				virtual property Platform::String^ Name;

				virtual void Install();
				virtual void Uninstall();

				bool Intersects(DistanceCollider^ other);
				Capstone::Core::Entity^ CollidesAgainst();
				bool Contains(float x, float y);
			};
		}
	}
}
