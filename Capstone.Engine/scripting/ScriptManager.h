#pragma once
#include "IScript.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Scripting
		{
			public ref class ScriptManager sealed
			{
			private:
				static ScriptManager^ _inst;
				ScriptManager();
				std::vector<IScript^> _scripts;

			public:
				static property ScriptManager^ Instance
				{
					ScriptManager^ get() 
					{
						if (_inst == nullptr)
							_inst = ref new ScriptManager();
						return _inst; 
					}
				}

				void RegisterScript(IScript^ script);
				void RemoveScript(IScript^ script);
				void ClearAllScripts();

				void Update();
				void PreDrawUpdate();
			};
		}
	}
}
