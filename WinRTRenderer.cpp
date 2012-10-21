#include "pch.h"
#include "WinRTRenderer.h"


WinRTRenderer::WinRTRenderer(void)
{
}


WinRTRenderer::~WinRTRenderer(void)
{
	delete _sb;
	_d3d = nullptr;
}

void WinRTRenderer::Draw(double elapsedSeconds)
{
	// Draw game layer first
	_sb->Begin();
	for (auto it = _gameLayer->begin(); it != _gameLayer->end(); ++it)
		(*it)->Draw(elapsedSeconds, _sb);
	_sb->End();
	return;

	// Then the UI layer
	_sb->Begin();
	for (auto it = _uiLayer->begin(); it != _uiLayer->end(); ++it)
		(*it)->Draw(elapsedSeconds, _sb);
	_sb->End();
}

void WinRTRenderer::Update(double elapsedSeconds)
{
	for (auto it = _gameLayer->begin(); it != _gameLayer->end(); ++it)
		(*it)->Update(elapsedSeconds);

	for (auto it = _uiLayer->begin(); it != _uiLayer->end(); ++it)
		(*it)->Update(elapsedSeconds);
}

void WinRTRenderer::LoadVisuals()
{
	for (auto it = _gameLayer->begin(); it != _gameLayer->end(); ++it)
		(*it)->Load(_d3d);

	for (auto it = _uiLayer->begin(); it != _uiLayer->end(); ++it)
		(*it)->Load(_d3d);
}
