#include "pch.h"
#include "ScriptManager.h"

namespace Capstone
{
	namespace Engine
	{
		namespace Scripting
		{
			ScriptManager^ ScriptManager::_inst;

			ScriptManager::ScriptManager()
			{
			}

			void ScriptManager::RegisterScript(IScript^ script)
			{
				_scripts.push_back(script);
				if (!script->IsInitialised)
					script->Initialise();
			}

			void ScriptManager::RemoveScript(IScript^ script)
			{
				for (auto i = _scripts.begin(); i != _scripts.end(); ++i)
				{
					if ((*i) == script)
					{
						_scripts.erase(i);
						script->Unload();
						break;
					}
				}
			}

			void ScriptManager::ClearAllScripts()
			{
				for (auto s : _scripts)
					s->Unload();
				_scripts.clear();
			}

			void ScriptManager::Update()
			{
				for (auto s : _scripts)
					s->Update();
			}

			void ScriptManager::PreDrawUpdate()
			{
				for (auto s: _scripts)
					s->PreDrawUpdate();
			}
		}
	}
}
