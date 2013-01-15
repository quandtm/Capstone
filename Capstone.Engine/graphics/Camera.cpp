#include "pch.h"
#include "Camera.h"

using namespace DirectX;

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

			void Camera::UpdateMatrices()
			{
				_view = XMMatrixTranslation(Entity->Translation->X, Entity->Translation->Y, 0);
			}
		}
	}
}
