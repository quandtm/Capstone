#pragma once
#include "Entity.h"

class AudioTrack
{
public:
	AudioTrack(void);
	~AudioTrack(void);

	void Load(wchar_t *path, IXAudio2 *engine);
	void Play();
	void Stop();

	void setEntity(Entity *e) { _entity = e; }

private:
	IXAudio2SourceVoice *_voice;
	BYTE *_data;
	int _buffLen;
	bool _ready;
	Entity *_entity;
};
