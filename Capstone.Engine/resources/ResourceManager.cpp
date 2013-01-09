#include "pch.h"
#include "ResourceManager.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Resources
		{
			ResourceManager::ResourceManager(Capstone::Engine::Memory::PageAllocator *allocator)
			{
				_memory = allocator;
			}

			ResourceManager::~ResourceManager()
			{
				for (auto i : _lookup)
				{
					i.second->Dispose();
					_memory->Deallocate(i.second);
				}
				_lookup.clear();
			}
		}
	}
}
