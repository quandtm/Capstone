#include "pch.h"
#include "PageAllocator.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Memory
		{
			PageAllocator::PageAllocator(uint32_t numPages, size_t pageSize)
				: _pageSize(pageSize), _numPages(numPages), _allocated(0)
			{
				_toc = new PageEntry[numPages];
				for (uint32_t i = 0; i < numPages; ++i)
				{
					_toc[i].IsAllocated = false;
					_toc[i].NumPages = 0;
				}
				_data = new BYTE[numPages * pageSize];
				if (_data != nullptr)
					_totalMemory = numPages * pageSize;
			}

			BYTE* PageAllocator::alloc(size_t requestedSize)
			{
				// We can't allocate more than the allocator has available
				if (requestedSize > Available()) return nullptr;

				// Find first contiguous block of pages
				size_t contig = 0;
				uint32_t startIndex = 0;
				for (uint32_t i = 0; i < _numPages; ++i)
				{
					if (_toc[i].IsAllocated) // If page is allocated
					{
						contig = 0;
						startIndex = i + 1;
					}
					else
					{
						contig += _pageSize;
						if (contig >= requestedSize)
							break;
					}
				}

				if (contig >= requestedSize)
				{
					for (uint32_t i = startIndex; i < (startIndex + (contig / _pageSize)); ++i)
						_toc[i].IsAllocated = true;
					_toc[startIndex].NumPages = contig / _pageSize;
					_allocated += contig;
					return &(_data[_pageSize * startIndex]);
				}
				else
					return nullptr;
			}

			void PageAllocator::dealloc(BYTE *startPtr)
			{
				auto page = (startPtr - _data) / _pageSize;
				auto size = _toc[page].NumPages;
				if (size > 0)
				{
					for (size_t i = page; i < page + size; ++i)
						_toc[i].IsAllocated = false;
					_allocated -= size * _pageSize;
				}
			}
		}
	}
}
