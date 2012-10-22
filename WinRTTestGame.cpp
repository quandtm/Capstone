#include "pch.h"
#include "WinRTTestGame.h"

WinRTTestGame::WinRTTestGame(void)
{
}

WinRTTestGame::~WinRTTestGame(void)
{
}

void WinRTTestGame::Initialise()
{
	e = new Entity();
	e->Translation.x = 100;
	e->Translation.y = 100;
	testSprite = _renderer->Create<Sprite>(e, LAYER_GAME, L"Assets\\Logo.png");
	_audioManager->Create(e, TRACKTYPE_MUSIC, L"Assets\\Lithium.mp3")->Play();
}

void WinRTTestGame::Update(double elapsedSeconds)
{
}
