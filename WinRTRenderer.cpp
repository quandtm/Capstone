#include "pch.h"
#include "WinRTRenderer.h"


WinRTRenderer::WinRTRenderer(void)
{
}


WinRTRenderer::~WinRTRenderer(void)
{
}

void WinRTRenderer::Draw(double elapsedSeconds)
{
	// TODO: Draw Sprites
}

void WinRTRenderer::Update(double elapsedSeconds)
{
	for (auto it = _gameLayer->begin(); it != _gameLayer->end(); ++it)
		(*it)->Update(elapsedSeconds);

	for (auto it = _uiLayer->begin(); it != _uiLayer->end(); ++it)
		(*it)->Update(elapsedSeconds);
}
