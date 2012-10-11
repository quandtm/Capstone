#pragma once
#include <DirectXMath.h>

class Entity
{
public:
	float Rotation;
	XMFLOAT2 Translation;
	XMFLOAT2 Scale;

	Entity()
	{
		Rotation = 0;
		Translation = XMFLOAT2(0, 0);
		Scale = XMFLOAT2(0, 0);
	}

	~Entity()
	{
	}
};
