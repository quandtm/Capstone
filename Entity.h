#pragma once

#include <DirectXMath.h>

class Entity
{
public:
	DirectX::XMFLOAT2 Translation;
	DirectX::XMFLOAT2 Scale;
	float Rotation;

	Entity();
	~Entity();
};
