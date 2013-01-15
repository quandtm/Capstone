#include "pch.h"
#include "Entity.h"

namespace Capstone
{
	namespace Core
	{
		Entity::Entity()
		{
			Translation->X = 0;
			Translation->Y = 0;
			Scale = 1;
			Rotation = 0;
		}

		void Entity::AddComponent(Platform::String^ key, IComponent^ component)
		{
			if (!key->IsEmpty())
			{
				_components[key] = component;
				component->Entity = this;
			}
		}

		IComponent^ Entity::GetComponent(Platform::String^ key)
		{
			return _components[key];
		}

		void Entity::RemoveComponent(Platform::String^ key)
		{
			if (!key->IsEmpty())
			{
				auto item = _components[key];
				if (item != nullptr)
				{
					_components.erase(key);
					item->Entity = nullptr;
				}
			}
		}
	}
}
