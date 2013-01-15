#pragma once

namespace Capstone
{
	namespace Core
	{
		public ref struct Vector2 sealed
		{
		public:
			property float X;
			property float Y;

			Vector2(void)
			{
				X = 0;
				Y = 0;
			}

			Vector2(float x, float y)
			{
				X = x;
				Y = y;
			}
		};
	}
}
