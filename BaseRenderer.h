#pragma once
#include "IVisual.h"
#include <vector>
#include "Entity.h"

enum LAYER
{
	LAYER_GAME,
	LAYER_UI
};

class BaseRenderer abstract
{
protected:
	std::vector<IVisual*> *_gameLayer;
	std::vector<IVisual*> *_uiLayer;

	void remove(std::vector<IVisual*>*, IVisual*);

public:
	BaseRenderer();
	~BaseRenderer();

	virtual void Draw(double elapsedSeconds) = 0;
	virtual void Update(double elapsedSeconds) = 0;

	template <class VisualType>
	VisualType* Create(Entity *e, LAYER layer, wchar_t *path)
	{
		auto v = new VisualType();
		v->setPath(path);
		v->setEntity(e);

		switch (layer)
		{
		case LAYER_GAME:
			_gameLayer->push_back(v);
			break;

		case LAYER_UI:
			_uiLayer->push_back(v);
			break;

		default:
			delete v;
			return nullptr;
		}

		return v;
	}

	void Remove(IVisual *visual, LAYER layer);
	virtual void LoadVisuals() = 0;
};
