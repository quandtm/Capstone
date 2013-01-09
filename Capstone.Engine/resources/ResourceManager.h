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
				bool Load(const std::wstring& path, ResType **ppRes)
				{
					auto res = _lookup[path];
					if (res == nullptr)
					{
						// Create and load
						auto mem = _memory->Allocate<ResType>();
						if (mem == nullptr) return false; // Allocation error, exiting
						auto data = new (mem) ResType();
						_lookup[path] = data;
						*ppRes = data;
						return data->Load(path);
					}
					else
					{
						*ppRes = static_cast<ResType*>(res);
						return res->IsReady();
					}
				}

				template<class ResType>
				inline Concurrency::task<bool> LoadAsync(const std::wstring& path, ResType **ppRes)
				{
					return concurrency::create_task([this, path, ppRes] { return this->Load(path, ppRes); });
				}
			};
		}
	}
}
