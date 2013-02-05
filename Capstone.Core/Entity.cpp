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
			Depth = 0;
		}

		void Entity::AddComponent(IComponent^ component)
		{
			auto key = component->Name;
			if (!key->IsEmpty() && component->Entity == nullptr)
			{
				_components[key] = component;
				component->Entity = this;
				component->Install();
			}
		}

		IComponent^ Entity::GetComponent(Platform::String^ key)
		{
			return _components[key];
		}

		void Entity::DestroyComponent(Platform::String^ key)
		{
			if (!key->IsEmpty())
			{
				auto component = _components[key];
				if (component != nullptr)
				{
					component->Uninstall();
					component->Entity = nullptr;
					_components.erase(key);
				}
			}
		}

		void Entity::DestroyComponents()
		{
			for(auto c : _components)
			{
				c.second->Uninstall();
				c.second->Entity = nullptr;
			}
			_components.clear();
		}
	}
}
