#pragma once
#include "IGame.h"

#include "Sprite.h"

class WinRTTestGame : public IGame
{
public:
	WinRTTestGame(void);
	~WinRTTestGame(void);

	void Initialise();
	void Update(double elapsedSeconds);

private:
	Sprite *testSprite;
	Entity *e;
};

