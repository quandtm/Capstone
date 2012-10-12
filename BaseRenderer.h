#pragma once
#include "IVisual.h"
#include <vector>

enum LAYER
{
	LAYER_GAME,
	LAYER_UI
};

class BaseRenderer
{
private:
	std::vector<IVisual*> *_gameLayer;
	std::vector<IVisual*> *_uiLayer;

public:
	BaseRenderer();
	~BaseRenderer();

	virtual void Draw(double elapsedSeconds) = 0;
	virtual void Update(double elapsedSeconds) = 0;

	template <class VisualType>
	VisualType* Create(LAYER layer)
	{
		auto v = new VisualType();

		switch (layer)
		{
		case LAYER_GAME:
			_gameLayer->push_back(v);
			break;

		case LAYER_UI:
			_gameLayer->push_back(v);
			break;

		default:
			delete v;
			return nullptr;
		}

		return v;
	}

	void Remove(IVisual *visual, LAYER layer);
};
