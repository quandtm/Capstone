#include "pch.h"
#include "Entity.h"

namespace Capstone
{
	namespace Core
	{
		Entity::Entity()
		{
			_trans.X = 0;
			_trans.Y = 0;
			Scale = 1;
			Rotation = 0;
		}

		void Entity::AddComponent(IComponent^ component)
		{
			auto key = component->Name;
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
