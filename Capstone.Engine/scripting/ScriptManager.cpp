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
				IsRunning = true;
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

			void ScriptManager::Update(float deltaTime, float totalTime)
			{
				if (IsRunning)
				{
					for (auto s : _scripts)
						s->Update(deltaTime, totalTime);
				}
			}

			void ScriptManager::PreDrawUpdate(float deltaTime, float totalTime)
			{
				if (IsRunning)
				{
					for (auto s : _scripts)
						s->PreDrawUpdate(deltaTime, totalTime);
				}
			}

			void ScriptManager::PointerPressed(float deltaTime, float totalTime, float x, float y)
			{
				if (IsRunning)
				{
					for (auto s : _scripts)
						s->PointerPressed(deltaTime, totalTime, x, y);
				}
			}

			void ScriptManager::PointerReleased(float deltaTime, float totalTime, float x, float y)
			{
				if (IsRunning)
				{
					for (auto s : _scripts)
						s->PointerReleased(deltaTime, totalTime, x, y);
				}
			}

			void ScriptManager::PointerMoved(float deltaTime, float totalTime, float x, float y)
			{
				if (IsRunning)
				{
					for (auto s : _scripts)
						s->PointerMoved(deltaTime, totalTime, x, y);
				}
			}
		}
	}
}
