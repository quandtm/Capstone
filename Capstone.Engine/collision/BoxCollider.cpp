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
				//CollisionManager::Instance->AddCollider(this);
			}

			void BoxCollider::Uninstall()
			{
				//CollisionManager::Instance->RemoveCollider(this);
			}

			bool BoxCollider::Intersects(BoxCollider^ other)
			{
				if (!CollideWithParent && other->Entity == Entity) return false;

				// TODO

				return false;
			}
		}
	}
}
