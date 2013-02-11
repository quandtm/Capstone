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
			}
		}
	}
}
