#pragma once
#include <DirectXMath.h>

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

			float DistanceTo(Vector2^ other)
			{
				auto posf = DirectX::XMFLOAT2(X, Y);
				auto otherPosf = DirectX::XMFLOAT2(other->X, other->Y);
				auto pos = DirectX::XMLoadFloat2(&posf);
				auto otherPos = DirectX::XMLoadFloat2(&otherPosf);
				auto dir = DirectX::XMVectorSubtract(otherPos, pos);
				auto distVec = DirectX::XMVector2Length(dir);
				float dist = -1.0f;
				DirectX::XMStoreFloat(&dist, distVec);
				return dist;
			}
		};
	}
}
