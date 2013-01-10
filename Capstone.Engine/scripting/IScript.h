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

				void Update();
				void PreDrawUpdate();
			};
		}
	}
}
