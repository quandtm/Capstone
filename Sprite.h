#pragma once
#include "ivisual.h"
#include "Direct3DBase.h"

class Sprite :
	public IVisual
{
public:
	Sprite(void);
	~Sprite(void);

	void Load(char *path);

	void Draw(double, DirectX::SpriteBatch*);
	void Update(double);

	void Load(wchar_t *path, Direct3DBase *d3d);

private:
	Microsoft::WRL::ComPtr<ID3D11Resource> _tex;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> _srv;
};

