#include "pch.h"
#include "Sprite.h"
#include "inc\WICTextureLoader.h"

Sprite::Sprite(void)
{
}


Sprite::~Sprite(void)
{
}

void Sprite::Load(char *path)
{
}

void Sprite::Draw(double elapsedSeconds, DirectX::SpriteBatch *sb)
{
	auto vec = DirectX::XMLoadFloat2(&entity->Translation);
	sb->Draw(_srv.Get(), vec);
}

void Sprite::Update(double elapsedSeconds)
{
}

void Sprite::Load(wchar_t *path, Direct3DBase *d3d)
{
	ID3D11Resource *res;
	ID3D11ShaderResourceView *srv;
	if (SUCCEEDED(DirectX::CreateWICTextureFromFile(d3d->GetDevice().Get(), d3d->GetDeviceContext().Get(), path, &res, &srv)))
	{
		_tex = res;
		_srv = srv;
	}
}
