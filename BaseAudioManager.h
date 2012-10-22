#pragma once
#include "AudioTrack.h"

enum TRACKTYPE
{
	TRACKTYPE_MUSIC,
	TRACKTYPE_SFX
};

class BaseAudioManager abstract
{
public:
	BaseAudioManager(void) { };
	~BaseAudioManager(void) { };

	virtual AudioTrack* Create(Entity *e, TRACKTYPE type, wchar_t *path) = 0;
};
