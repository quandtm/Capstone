#include "pch.h"
#include "CameraManager.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Graphics
		{
			CameraManager^ CameraManager::_inst;

			void CameraManager::AddCamera(Platform::String^ name, Camera^ cam)
			{
				if (_cameras[name] == nullptr)
					_cameras[name] = cam;
			}

			void CameraManager::RemoveCamera(Platform::String^ name)
			{
				auto cam = _cameras[name];
				if (cam != nullptr)
				{
					_cameras.erase(name);
					if (cam == _active)
						_active = nullptr;
				}
			}

			void CameraManager::MakeActive(Platform::String^ name)
			{
				auto cam = _cameras[name];
				if (cam != nullptr && cam != _active)
					_active = cam;
			}

			Camera^ CameraManager::GetCamera(Platform::String^ name)
			{
				return _cameras[name];
			}

			void CameraManager::Update(float screenWidth, float screenHeight)
			{
				if (ActiveCamera != nullptr)
					ActiveCamera->UpdateMatrices(screenWidth, screenHeight);
			}
		}
	}
}
