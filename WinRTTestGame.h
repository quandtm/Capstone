#pragma once
#include "IGame.h"

class WinRTTestGame : public IGame
{
public:
	WinRTTestGame(void);
	~WinRTTestGame(void);

	void Initialise();
	void Load();
	void Update(double elapsedSeconds);
};

