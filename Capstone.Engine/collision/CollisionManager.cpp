#include "pch.h"
#include "CollisionManager.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Collision
		{
			CollisionManager^ CollisionManager::_inst;

			CollisionManager::CollisionManager()
			{
			}

			void CollisionManager::AddCollider(BoxCollider^ collider)
			{
				_colliders.push_back(collider);
			}
		}
	}
}
