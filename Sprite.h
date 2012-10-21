#pragma once
#include "ivisual.h"
#include "Direct3DBase.h"

class Sprite :
	public IVisual
{
public:
	DirectX::XMFLOAT2 Origin;

	Sprite(void);
	~Sprite(void);

	void Draw(double, DirectX::SpriteBatch*);
	void Update(double);

	void Load(Direct3DBase *d3d);

private:
	Microsoft::WRL::ComPtr<ID3D11Resource> _tex;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> _srv;
	bool _isLoaded;
};

