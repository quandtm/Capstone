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

			void CollisionManager::RemoveCollider(BoxCollider^ collider)
			{
				std::vector<BoxCollider^>::iterator target;
				bool found = false;
				for (auto it = _colliders.begin(); it != _colliders.end(); ++it)
				{
					if ((*it) == collider)
					{
						target = it;
						found = true;
						break;
					}
				}
				if (found)
					_colliders.erase(target);
			}
		}
	}
}
