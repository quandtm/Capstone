#pragma once

#include <wrl/client.h>
#include <d3d11_1.h>
#include <DirectXMath.h>
#include <memory>
#include <agile.h>

#if _DEBUG
#pragma comment(lib, "DirectXTK_d.lib")
#else
#pragma comment(lib, "DirectXTK.lib")
#endif
#include <SpriteBatch.h>

#pragma comment(lib, "xaudio2.lib")
#include <xaudio2.h>

typedef struct
{
	UINT32 a, b, c, d;
} LGUID;

char* GuidToStr(LGUID);
LGUID StrToGuid(char*);
