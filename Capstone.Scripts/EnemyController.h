#pragma once

namespace Capstone
{
	namespace Scripts
	{
		using namespace Platform;
		using namespace Capstone::Core;
		using namespace Capstone::Engine::Scripting;
		using namespace Capstone::Engine::Collision;

		public ref class EnemyController sealed : public Capstone::Engine::Scripting::IScript
		{
		private:
			bool _isInitialised;
			DistanceCollider^ _huntCollider;
			DistanceCollider^ _attackCollider;
			bool _hunting;
			Entity^ _target;

		public:
			EnemyController();

			[Capstone::Core::ComponentParameterAttribute(DisplayName="Weapon Range")]
			property float WeaponRange;
			[Capstone::Core::ComponentParameterAttribute(DisplayName="Speed")]
			property float Speed;
			[Capstone::Core::ComponentParameterAttribute(DisplayName="Hunt Range")]
			property float HuntRange;
			[Capstone::Core::ComponentParameterAttribute(DisplayName="Weapon DPS")]
			property float WeaponDPS;

			// Properties
			virtual property Entity^ Entity;
			virtual property String^ Name;
			property bool IsInitialised
			{
				virtual bool get() { return _isInitialised; }
			}

			// IComponent Methods
			virtual void Install();
			virtual void Uninstall();

			// IScript Methods
			virtual void Initialise();
			virtual void Unload();
			virtual void Update(float deltaTime, float totalTime);
			virtual void PreDrawUpdate(float deltaTime, float totalTime);
			virtual void PointerPressed(float deltaTime, float totalTime, float x, float y);
			virtual void PointerMoved(float deltaTime, float totalTime, float x, float y);
			virtual void PointerReleased(float deltaTime, float totalTime, float x, float y);
		};
	}
}
