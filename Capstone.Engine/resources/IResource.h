#pragma once

namespace Capstone
{
	namespace Engine
	{
		namespace Resources
		{
			class IResource abstract
			{
			protected:
				std::atomic_bool _isReady;

			public:
				IResource()
				{
					_isReady = false;
				};

				virtual bool Load(const std::wstring& path) = 0;
				virtual void Dispose() = 0;
				bool IsReady() { return _isReady; }
			};
		}
	}
}
