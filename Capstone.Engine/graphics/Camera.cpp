#include "pch.h"
#include "Camera.h"
#include "CameraManager.h"

using namespace DirectX;
using namespace Capstone::Core;

namespace Capstone
{
	namespace Engine
	{
		namespace Graphics
		{
			Camera::Camera()
			{
				_view = XMMatrixIdentity();
			}

			void Camera::Setup()
			{
				CameraManager::Instance->AddCamera(Name, this);
			}

			void Camera::Destroy()
			{
				CameraManager::Instance->RemoveCamera(Name);
			}

			void Camera::UpdateMatrices()
			{
				_view = XMMatrixTranslation(-Entity->Translation->X, -Entity->Translation->Y, 0);
			}

			void Camera::ScreenToWorld(Vector2^ screen, Vector2^ world)
			{
				auto flt = XMFLOAT2(screen->X, screen->Y);
				_declspec(align(16)) auto vec = XMLoadFloat2(&flt);
				auto det = XMMatrixDeterminant(_view);
				auto inv = XMMatrixInverse(&det, _view);
				_declspec(align(16)) auto result = XMVector2TransformCoord(vec, inv);
				auto worldFlt = XMFLOAT2();
				XMStoreFloat2(&worldFlt, result);
				world->X = worldFlt.x;
				world->Y = worldFlt.y;
			}
		}
	}
}
