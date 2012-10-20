#pragma once
#include "IGame.h"

class WinRTTestGame : IGame
{
public:
	WinRTTestGame(void);
	~WinRTTestGame(void);

	void Initialise();
	void Load();
	void Update(double elapsedSeconds);
};

