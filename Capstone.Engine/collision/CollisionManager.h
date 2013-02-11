#pragma once
#include "DistanceCollider.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Collision
		{
			public ref class CollisionManager sealed
			{
			private:
				std::vector<DistanceCollider^> _colliders;

				static CollisionManager^ _inst;
				CollisionManager();
			public:
				static property CollisionManager^ Instance
				{
					CollisionManager^ get()
					{
						if (_inst == nullptr)
							_inst = ref new CollisionManager();
						return _inst;
					}
				}

				void AddCollider(DistanceCollider^ collider);
				void RemoveCollider(DistanceCollider^ collider);

				Entity^ Collide(DistanceCollider^ collider);
			};
		}
	}
}
