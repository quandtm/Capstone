#include "pch.h"
#include "BoxCollider.h"
#include "CollisionManager.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Collision
		{
			void BoxCollider::Install()
			{
				CollisionManager::Instance->AddCollider(this);
			}

			void BoxCollider::Uninstall()
			{
				CollisionManager::Instance->RemoveCollider(this);
			}

			bool BoxCollider::IsCollidingWith(BoxCollider^ other)
			{
				return false;
			}

			bool BoxCollider::IsPointInCollider(float x, float y)
			{
				return true;
			}
		}
	}
}
