#pragma once
#include "Entity.h"
#include "Direct3DBase.h"

class IVisual abstract
{
protected:
	Entity *entity;
	wchar_t *assetPath;

public:
	virtual ~IVisual() {};
	virtual void Draw(double elapsedSeconds, DirectX::SpriteBatch *sb) = 0;
	virtual void Update(double elapsedSeconds) = 0;

	virtual void Load(Direct3DBase *d3d) = 0;
	
	void setPath(wchar_t *path)
	{
		assetPath = path;
	}

	void setEntity(Entity *e)
	{
		entity = e;
	}
};
