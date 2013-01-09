#pragma once

#include <wrl/client.h>
#include <d3d11_1.h>
#include <DirectXMath.h>
#include <memory>
#include <agile.h>
#include <string>
#include <map>
#include <ppl.h>
#include <ppltasks.h>
#include <atomic>

#if NDEBUG
#pragma comment(lib, "DirectXTK.lib")
#else
#pragma comment(lib, "DirectXTK_d.lib")
#endif
