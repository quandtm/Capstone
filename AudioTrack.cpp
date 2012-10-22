#include "pch.h"
#include "AudioTrack.h"

AudioTrack::AudioTrack(void)
{
	_ready = false;
}

AudioTrack::~AudioTrack(void)
{
}

void AudioTrack::Load(wchar_t *path, IXAudio2 *engine)
{
	WAVEFORMATEX fmt;
	// TODO: Load audio
	if (SUCCEEDED(engine->CreateSourceVoice(&_voice, &fmt)))
	{
		_ready = true;
	}
}
