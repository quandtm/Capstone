#pragma once
#include "Camera.h"
#include <map>

namespace Capstone
{
	namespace Engine
	{
		namespace Graphics
		{
			public ref class CameraManager sealed
			{
			private:
				std::map<Platform::String^, Camera^> _cameras;
				Camera^ _active;

				CameraManager(void) : _active(nullptr) { };
				static CameraManager^ _inst;

			public:
				static property CameraManager^ Instance
				{
					CameraManager^ get()
					{
						if (_inst == nullptr)
							_inst = ref new CameraManager();
						return _inst;
					}
				}

				property Camera^ ActiveCamera { Camera^ get() { return _active; } }

				void AddCamera(Platform::String^ name, Camera^ cam);
				void RemoveCamera(Platform::String^ name);
				void MakeActive(Platform::String^ name);

				void Update(float screenWidth, float screenHeight);
			};
		}
	}
}
