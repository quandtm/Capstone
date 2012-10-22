#include "pch.h"
#include "AudioTrack.h"

AudioTrack::AudioTrack(void)
{
	_ready = false;
}

AudioTrack::~AudioTrack(void)
{
}

void AudioTrack::Play()
{
	if (!_ready) return;

	XAUDIO2_BUFFER buff = {0};

	_voice->Stop();
	_voice->FlushSourceBuffers();

	buff.AudioBytes = _buffLen;
	buff.pAudioData = _data;
	buff.Flags = XAUDIO2_END_OF_STREAM;

	_voice->SubmitSourceBuffer(&buff);
	_voice->Start();
}

void AudioTrack::Stop()
{
	if (!_ready) return;

	_voice->Stop();
}

void AudioTrack::Load(wchar_t *path, IXAudio2 *engine)
{
	WAVEFORMATEX fmt;
	ZeroMemory(&fmt, sizeof(fmt));
	
	// TODO: Load audio

	if (SUCCEEDED(engine->CreateSourceVoice(&_voice, &fmt)))
	{
		_ready = true;
	}
}
