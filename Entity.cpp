#include "pch.h"
#include "Entity.h"

Entity::Entity()
{
	Translation = DirectX::XMFLOAT2(0, 0);
	Scale = DirectX::XMFLOAT2(0, 0);
	Rotation = 0;
}

Entity::~Entity()
{
}
