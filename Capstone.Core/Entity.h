#pragma once
#include "IComponent.h"
#include "Vector2.h"

namespace Capstone
{
	namespace Core
	{
		public ref class Entity sealed
		{
		private:
			std::map<Platform::String^, IComponent^> _components;

		public:
			Entity();

			property Vector2^ Translation;
			property float Scale;
			property float Rotation;

			void AddComponent(Platform::String^ key, IComponent^ component);
			IComponent^ GetComponent(Platform::String^ key);
			void RemoveComponent(Platform::String^ key);
		};
	}
}
