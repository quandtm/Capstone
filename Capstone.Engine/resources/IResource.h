#pragma once

namespace Capstone
{
	namespace Engine
	{
		namespace Resources
		{
			typedef enum
			{
				ResourceStatus_None = 0,
				ResourceStatus_AllocationError = -1,
				ResourceStatus_LoadingError = -1,
				ResourceStatus_Loading = 1,
				ResourceStatus_Loaded = 2
			} ResourceStatus;

			class IResource abstract
			{
			protected:
				std::atomic<ResourceStatus> _status;

			public:
				IResource()
				{
					_status = ResourceStatus_None;
				};

				virtual ResourceStatus Load(const std::wstring& path) = 0;
				virtual void Dispose() = 0;
				ResourceStatus GetStatus() { return _status; }
			};
		}
	}
}
