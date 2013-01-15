#pragma once
#include "IResource.h"
#include "../memory/PageAllocator.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Resources
		{

			class ResourceManager sealed
			{
			private:
				std::map<std::wstring, IResource*> _lookup;
				Capstone::Engine::Memory::PageAllocator *_memory;

			public:
				ResourceManager(Capstone::Engine::Memory::PageAllocator *allocator);
				~ResourceManager();

				template<class ResType>
				Concurrency::task<ResourceStatus> LoadAsync(const std::wstring& path, ResType **ppRes)
				{
					auto res = _lookup[path];
					if (res == nullptr)
					{
						// Create and load
						auto mem = _memory->Allocate<ResType>();
						if (mem == nullptr) return concurrency::create_task([] { return ResourceStatus_AllocationError; }); // Allocation error, exiting
						auto data = new (mem) ResType();
						_lookup[path] = data;
						if (ppRes != nullptr)
							*ppRes = data;
						return concurrency::create_task(
							[this, path, data]
						{ 
							return data->Load(path);
						});
					}
					else
					{
						if (ppRes != nullptr)
							*ppRes = static_cast<ResType*>(res);
						return concurrency::create_task(
							[res]
						{
							return res->GetStatus();
						});
					}
				}
			};
		}
	}
}
