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

				BYTE* alloc(const size_t requestedSize);
				void dealloc(BYTE *startPtr);

				size_t _totalMemory;
				size_t _allocated;

			public:
				PageAllocator(const uint32_t numPages, const size_t pageSize);

				inline const size_t TotalSize() const { return _totalMemory; };
				inline const size_t Available() const { return _totalMemory - _allocated; };
				inline const size_t Allocated() const { return _allocated; };

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
