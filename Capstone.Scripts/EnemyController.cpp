#include "pch.h"
#include "EnemyController.h"
#include "PlayerController.h"

namespace Capstone
{
	namespace Scripts
	{
		using namespace DirectX;

		EnemyController::EnemyController()
		{
		}

		void EnemyController::Install()
		{
			ScriptManager::Instance->RegisterScript(this);
		}

		void EnemyController::Uninstall()
		{
			ScriptManager::Instance->RemoveScript(this);
		}

		void EnemyController::Initialise()
		{
			_huntCollider = ref new DistanceCollider();
			_huntCollider->Name = "huntCollider";
			_huntCollider->Distance = HuntRange;
			_attackCollider = ref new DistanceCollider();
			_attackCollider->Name = "attackCollider";
			_attackCollider->Distance = WeaponRange;

			Entity->AddComponent(_huntCollider);
			Entity->AddComponent(_attackCollider);

			_hunting = false;

			_isInitialised = true;
		}

		void EnemyController::Unload()
		{
			Entity->DestroyComponent("huntCollider");
			Entity->DestroyComponent("attackCollider");
		}

		void EnemyController::Update(float deltaTime, float totalTime)
		{
			if (_hunting)
			{
				// Moving to/attacking player
				// Check the player is still in range
				auto hc = _huntCollider->CollidesAgainst();
				if (hc == nullptr)
				{
					_hunting = false;
					return;
				}

				// If we're in range to do damage, hurt the player
				auto pc = _target->GetComponentFromType("Capstone.Engine.Collision.DistanceCollider");
				if (pc != nullptr && _attackCollider->Intersects((DistanceCollider^)pc))
				{
					auto p = _target->GetComponentFromType("Capstone.Scripts.PlayerController");
					if (p != nullptr)
						((PlayerController^)p)->TakeDamage(WeaponDPS * deltaTime);
				}
				else
				{
					// Move towards player until in range
					XMFLOAT2 dest = XMFLOAT2(_target->Translation->X, _target->Translation->Y);
					auto targetPos = XMLoadFloat2(&dest);
					XMFLOAT2 me = XMFLOAT2(Entity->Translation->X, Entity->Translation->Y);
					auto curPos = XMLoadFloat2(&me);
					auto dir = targetPos - curPos;
					auto nDir = XMVector2Normalize(dir);
					auto dist = nDir * (Speed * deltaTime);
					XMFLOAT2 distFlt;
					XMStoreFloat2(&distFlt, dist);
					Entity->Translation->X = Entity->Translation->X + distFlt.x;
					Entity->Translation->Y = Entity->Translation->Y + distFlt.y;
				}
			}
			else
			{
				// Idling
				auto c = _huntCollider->CollidesAgainst();
				if (c != nullptr)
				{
					auto pc = c->GetComponentFromType("Capstone.Scripts.PlayerController");
					if (pc != nullptr)
					{
						_target = c;
						_hunting = true;
					}
				}
			}
		}

		void EnemyController::PreDrawUpdate(float deltaTime, float totalTime)
		{
		}

		void EnemyController::PointerPressed(float deltaTime, float totalTime, float x, float y)
		{
		}

		void EnemyController::PointerMoved(float deltaTime, float totalTime, float x, float y)
		{
		}

		void EnemyController::PointerReleased(float deltaTime, float totalTime, float x, float y)
		{
		}
	}
}
