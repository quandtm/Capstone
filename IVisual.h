#pragma once
#include "Entity.h"

class IVisual
{
protected:
	Entity *entity;

public:
	virtual ~IVisual() {};
	virtual void Draw(double elapsedSeconds, DirectX::SpriteBatch *sb) = 0;
	virtual void Update(double elapsedSeconds) = 0;

	/*void SetOwner(BaseRenderer *renderer)
	{
		owner = renderer;
	}*/
};
