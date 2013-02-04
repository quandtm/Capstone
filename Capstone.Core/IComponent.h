#pragma once
#include "Entity.h"

namespace Capstone
{
	namespace Core
	{
		ref class Entity;

		public interface class IComponent
		{
		public:
			property Entity^ Entity;
			property Platform::String^ Name;
			void Setup();
			void Destroy();
		};
	}
}
