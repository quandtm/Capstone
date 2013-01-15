#pragma once
#include "IComponent.h"

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

			property float TranslationX;
			property float TranslationY;
			property float Scale;
			property float Rotation;

			void AddComponent(Platform::String^ key, IComponent^ component);
			IComponent^ GetComponent(Platform::String^ key);
			void RemoveComponent(Platform::String^ key);
		};
	}
}
