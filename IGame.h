#pragma once
#include "BaseRenderer.h"

// This handles the logic for the game, and shouldn't draw or do much with the provided BaseRenderer and AudioManager objects
class IGame
{
public:
	virtual ~IGame() {};
	virtual void Initialise() = 0;
	virtual void Load() = 0;
	virtual void Update(double elapsedSeconds) = 0;

	void setRenderer(BaseRenderer *value) { _renderer = value; }

protected:
	BaseRenderer *_renderer;
};
