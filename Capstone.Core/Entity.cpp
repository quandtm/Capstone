#include "pch.h"
#include "Entity.h"

namespace Capstone
{
	namespace Core
	{
		Entity::Entity()
		{
			TranslationX = 0;
			TranslationY = 0;
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
	}
}
