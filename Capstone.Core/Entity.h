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
			Vector2 _trans;

		public:
			Entity();

			property Vector2^ Translation { Vector2^ get() { return %_trans; } };
			property float Scale;
			property float Rotation;

			void AddComponent(Platform::String^ key, IComponent^ component);
			IComponent^ GetComponent(Platform::String^ key);
			void RemoveComponent(Platform::String^ key);
		};
	}
}
