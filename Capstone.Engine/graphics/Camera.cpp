#include "pch.h"
#include "Camera.h"

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

			void Camera::UpdateMatrices()
			{
				_view = XMMatrixTranslation(Entity->Translation->X, Entity->Translation->Y, 0);
			}

			void Camera::ScreenToWorld(Vector2^ screen, Vector2^ world)
			{
				// TODO: Actually transform
				world->X = screen->X;
				world->Y = screen->Y;
			}
		}
	}
}
