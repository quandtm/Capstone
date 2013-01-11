#pragma once

namespace Capstone
{
	namespace Engine
	{
		namespace Scripting
		{
			public interface class IScript : public Capstone::Core::IComponent
			{
			public:
				void Initialise();
				property bool IsInitialised
				{
					bool get();
				}
				void Unload();

				void Update(float deltaTime, float totalTime);
				void PreDrawUpdate(float deltaTime, float totalTime);

				void PointerPressed(float deltaTime, float totalTime, float x, float y);
				void PointerMoved(float deltaTime, float totalTime, float x, float y);
				void PointerReleased(float deltaTime, float totalTime, float x, float y);
			};
		}
	}
}
