#include "pch.h"
#include "Sprite.h"
#include "inc\WICTextureLoader.h"

Sprite::Sprite(void)
{
	Origin = DirectX::XMFLOAT2(0, 0);
	_isLoaded = false;
}


Sprite::~Sprite(void)
{
}

void Sprite::Draw(double elapsedSeconds, DirectX::SpriteBatch *sb)
{
	if (_isLoaded)
		sb->Draw(_srv.Get(), entity->Translation);
		//sb->Draw(_srv.Get(), entity->Translation, nullptr, DirectX::Colors::White, entity->Rotation, Origin, entity->Scale);
}

void Sprite::Update(double elapsedSeconds)
{
}

void Sprite::Load(Direct3DBase *d3d)
{
	ID3D11Resource *res;
	ID3D11ShaderResourceView *srv;
	HRESULT hr;
	if (SUCCEEDED(hr = DirectX::CreateWICTextureFromFile(d3d->GetDevice().Get(), d3d->GetDeviceContext().Get(), assetPath, &res, &srv)))
	{
		_tex = res;
		_srv = srv;
		_isLoaded = true;
	}
}
