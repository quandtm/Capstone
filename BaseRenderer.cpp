#include "pch.h"
#include "BaseRenderer.h"

BaseRenderer::BaseRenderer()
{
	_gameLayer = new std::vector<IVisual*>();
	_uiLayer = new std::vector<IVisual*>();
}

BaseRenderer::~BaseRenderer()
{
	delete _gameLayer;
	delete _uiLayer;
}

void BaseRenderer::Remove(IVisual *visual, LAYER layer)
{
	// TODO
}
