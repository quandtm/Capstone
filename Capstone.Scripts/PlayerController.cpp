#include "pch.h"
#include "PlayerController.h"

using namespace DirectX;

namespace Capstone
{
	namespace Scripts
	{
		using namespace Capstone::Engine::Collision;

		PlayerController::PlayerController()
		{
		}

		void PlayerController::Install()
		{
			ScriptManager::Instance->RegisterScript(this);

		}

		void PlayerController::Uninstall()
		{
			ScriptManager::Instance->RemoveScript(this);
		}

		void PlayerController::Initialise()
		{
			_moving = false;
			_dest = XMFLOAT2(0, 0);

			_isInitialised = true;
		}

		void PlayerController::Unload()
		{
		}

		void PlayerController::Update(float deltaTime, float totalTime)
		{
			if (_moving)
			{
				auto curPos = XMFLOAT2(Entity->Translation->X, Entity->Translation->Y);
				auto dVec = XMLoadFloat2(&_dest);
				auto curVec = XMLoadFloat2(&curPos);
				auto dir = dVec - curVec;
				auto dirNorm = XMVector2Normalize(dir);
				dir = dirNorm * (Speed * deltaTime);
				XMFLOAT2 delta;
				XMStoreFloat2(&delta, dir);
				Entity->Translation->X = Entity->Translation->X + delta.x;
				Entity->Translation->Y = Entity->Translation->Y + delta.y;

				// Determine if we need to stop
				curPos.x = Entity->Translation->X;
				curPos.y = Entity->Translation->Y;
				curVec = XMLoadFloat2(&curPos);
				auto distVec = dVec - curVec;
				auto distResult = XMVector2Length(distVec);
				float dist = 0;
				XMStoreFloat(&dist, distResult);
				if(dist <= StopRadius)
					_moving = false;
			}
		}

		void PlayerController::PreDrawUpdate(float deltaTime, float totalTime)
		{
		}

		void PlayerController::PointerPressed(float deltaTime, float totalTime, float x, float y)
		{
		}

		void PlayerController::PointerMoved(float deltaTime, float totalTime, float x, float y)
		{
		}

		void PlayerController::PointerReleased(float deltaTime, float totalTime, float x, float y)
		{
			auto scr = ref new Vector2(x, y);
			auto world = ref new Vector2(x, y);
			Capstone::Engine::Graphics::CameraManager::Instance->ActiveCamera->ScreenToWorld(scr, world);

			auto clicked = CollisionManager::Instance->PointInCollider(world->X, world->Y);
			if (clicked == nullptr)
			{
				_dest.x = world->X;
				_dest.y = world->Y;
				_moving = true;
			}
			else
			{
				auto ec = clicked->GetComponentFromType("Capstone.Scripts.EnemyController");
				if (ec != nullptr)
				{
					// This is an enemy, attack it
					auto dist = Entity->Translation->DistanceTo(clicked->Translation);
					if (dist < CloseAttackRange)
					{
						// TODO: Check cooldown then attack
					}
				}
			}
		}

		void PlayerController::TakeDamage(float damage)
		{
			HP = HP - damage;
		}
	}
}
