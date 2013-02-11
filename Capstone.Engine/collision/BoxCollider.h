#pragma once

namespace Capstone
{
	namespace Engine
	{
		namespace Collision
		{
			using namespace Capstone::Core;
			using namespace DirectX;

			// AABB
			public ref class BoxCollider sealed : public IComponent
			{
			private:
				XMFLOAT2 _topLeft;
				XMFLOAT2 _size;

			public:
				[Capstone::Core::ComponentParameterAttribute(DisplayName="X")]
				property float X
				{
					float get() { return _topLeft.x; }
					void set(float val) { _topLeft.x = val; }
				}
				[Capstone::Core::ComponentParameterAttribute(DisplayName="Y")]
				property float Y
				{
					float get() { return _topLeft.y; }
					void set(float val) { _topLeft.y = val; }
				}
				[Capstone::Core::ComponentParameterAttribute(DisplayName="Width")]
				property float Width
				{
					float get() { return _size.x; }
					void set(float val) { _size.x = val; }
				}
				[Capstone::Core::ComponentParameterAttribute(DisplayName="Height")]
				property float Height
				{
					float get() { return _size.y; }
					void set(float val) { _size.y = val; }
				}

				[Capstone::Core::ComponentParameterAttribute(DisplayName="Collide with Parent Entity")]
				property bool CollideWithParent;

				virtual property Entity^ Entity;
				virtual property Platform::String^ Name;

				BoxCollider()
					: _topLeft(XMFLOAT2(0, 0)), _size(XMFLOAT2(0, 0))
				{}
				BoxCollider(float x, float y, float width, float height)
					: _topLeft(XMFLOAT2(x, y)), _size(XMFLOAT2(width, height))
				{}

				virtual void Install();
				virtual void Uninstall();

				bool Intersects(BoxCollider^ other);
			};
		}
	}
}
