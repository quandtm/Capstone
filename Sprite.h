#pragma once
#include "ivisual.h"

class Sprite :
	public IVisual
{
public:
	Sprite(void);
	~Sprite(void);

	void Load(char *path);

	void Draw(double, DirectX::SpriteBatch*);
	void Update(double);
};

