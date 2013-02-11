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

			void CollisionManager::AddCollider(DistanceCollider^ collider)
			{
				_colliders.push_back(collider);
			}

			void CollisionManager::RemoveCollider(DistanceCollider^ collider)
			{
				std::vector<DistanceCollider^>::iterator target;
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

			Entity^ CollisionManager::Collide(DistanceCollider^ collider)
			{
				for (auto c : _colliders)
				{
					if (c != collider)
					{
						if (collider->Intersects(c))
							return c->Entity;
					}
				}
				return nullptr;
			}
		}
	}
}
