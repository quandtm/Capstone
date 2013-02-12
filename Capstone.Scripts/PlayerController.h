#pragma once

namespace Capstone
{
	namespace Scripts
	{
		using namespace Platform;
		using namespace Capstone::Core;
		using namespace Capstone::Engine::Scripting;

		public ref class PlayerController sealed : public Capstone::Engine::Scripting::IScript
		{
		private:
			bool _isInitialised;
			DirectX::XMFLOAT2 _dest;
			bool _moving;
			float _health;

			bool IsAlive() { return _health > 0; }

		public:
			PlayerController();

			[Capstone::Core::ComponentParameterAttribute(DisplayName="Stopping Radius")]
			property float StopRadius;
			[Capstone::Core::ComponentParameterAttribute(DisplayName="Speed")]
			property float Speed;
			[Capstone::Core::ComponentParameterAttribute(DisplayName="Health")]
			property float HP
			{
				float get() { return _health; }
				void set(float val)
				{
					if (_health == val) return;
					_health = val;
					HealthChanged(this, ref new Windows::UI::Xaml::Data::PropertyChangedEventArgs("HP"));
				}
			}
			[Capstone::Core::ComponentParameterAttribute(DisplayName="Melee Range")]
			property float CloseAttackRange;
			[Capstone::Core::ComponentParameterAttribute(DisplayName="Melee Damage")]
			property float CloseAttackDamage;
			[Capstone::Core::ComponentParameterAttribute(DisplayName="Melee Cooldown (seconds)")]
			property float CloseAttackCooldown;

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

			event Windows::UI::Xaml::Data::PropertyChangedEventHandler^ HealthChanged;

			void TakeDamage(float damage);
		};
	}
}
