#pragma once

namespace Capstone
{
	namespace Engine
	{
		namespace Memory
		{
			typedef struct
			{
				bool IsAllocated;
				size_t NumPages;
			} PageEntry;

			class PageAllocator sealed
			{
			private:
				PageEntry *_toc; // Indicates which pages have been allocated
				BYTE *_data;

				size_t _pageSize;
				uint32_t _numPages;

				BYTE* alloc(size_t requestedSize);
				void dealloc(BYTE *startPtr);

				size_t _totalMemory;
				size_t _allocated;

			public:
				PageAllocator(uint32_t numPages, size_t pageSize);

				inline size_t TotalSize() { return _totalMemory; }
				inline size_t Available() { return _totalMemory - _allocated; }
				inline size_t Allocated() { return _allocated; }

				template<class T>
				inline T* Allocate()
				{
					return reinterpret_cast<T*>(alloc(sizeof(T)));
				}

				template<class T>
				inline void Deallocate(T *ptr)
				{
					dealloc(reinterpret_cast<BYTE*>(ptr));
				}
			};
		}
	}
}
