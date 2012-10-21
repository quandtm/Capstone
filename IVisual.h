#pragma once
#include "Entity.h"
#include "Direct3DBase.h"

class IVisual
{
protected:
	Entity *entity;

public:
	virtual ~IVisual() {};
	virtual void Draw(double elapsedSeconds, DirectX::SpriteBatch *sb) = 0;
	virtual void Update(double elapsedSeconds) = 0;

	virtual void Load(wchar_t *path, Direct3DBase *d3d) = 0;
};
