#pragma once
#include "baserenderer.h"
#include "Direct3DBase.h"

class WinRTRenderer :
	public BaseRenderer
{
public:
	WinRTRenderer(void);
	~WinRTRenderer(void);

	void setD3DBase(Direct3DBase *base)
	{
		_d3d = base;
		_ctx = _d3d->GetDeviceContext();
	}

	void Draw(double elapsedSeconds);
	void Update(double elapsedSeconds);

private:
	Direct3DBase *_d3d;
	Microsoft::WRL::ComPtr<ID3D11DeviceContext1> _ctx;
};

