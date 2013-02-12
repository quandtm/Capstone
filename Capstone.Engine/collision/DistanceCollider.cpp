#include "pch.h"
#include "DistanceCollider.h"
#include "CollisionManager.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Collision
		{
			using namespace DirectX;

			void DistanceCollider::Install()
			{
				CollisionManager::Instance->AddCollider(this);
			}

			void DistanceCollider::Uninstall()
			{
				CollisionManager::Instance->RemoveCollider(this);
			}

			bool DistanceCollider::Intersects(DistanceCollider^ other)
			{
				// RTR v3 p.763
				auto thisCenter = XMFLOAT2(Entity->Translation->X + OffsetX, Entity->Translation->Y + OffsetY);
				auto otherCenter = XMFLOAT2(other->Entity->Translation->X + other->OffsetX, other->Entity->Translation->Y + other->OffsetY);
				auto tv = XMLoadFloat2(&thisCenter);
				auto ov = XMLoadFloat2(&otherCenter);
				auto h = tv - ov;
				auto d2v = XMVector2Dot(h, h);
				float d2 = 0;
				XMStoreFloat(&d2, d2v);
				auto r = Distance + other->Distance;
				if (d2 > (r * r)) return false;
				return true;
			}

			Entity^ DistanceCollider::CollidesAgainst()
			{
				return CollisionManager::Instance->Collide(this);
			}

			bool DistanceCollider::Contains(float x, float y)
			{
				auto posF = XMFLOAT2(Entity->Translation->X + OffsetX, Entity->Translation->Y + OffsetY);
				auto ptF = XMFLOAT2(x, y);
				auto pos = XMLoadFloat2(&posF);
				auto pt = XMLoadFloat2(&ptF);
				auto v = pt - pos;
				auto distVec = XMVector2LengthSq(v);
				float dist;
				XMStoreFloat(&dist, distVec);
				return dist < (Distance * Distance);
			}
		}
	}
}
