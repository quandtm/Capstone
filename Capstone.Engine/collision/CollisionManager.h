#pragma once
#include "BoxCollider.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Collision
		{
			public ref class CollisionManager sealed
			{
			private:
				std::vector<BoxCollider^> _colliders;

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

				void AddCollider(BoxCollider^ collider);
			};
		}
	}
}
