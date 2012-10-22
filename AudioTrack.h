#pragma once
class AudioTrack
{
public:
	AudioTrack(void);
	~AudioTrack(void);

	void Load(wchar_t *path, IXAudio2 *engine);
	void Play();

private:
	IXAudio2SourceVoice *_voice;
	BYTE *_data;
	int _buffLen;
	bool _ready;
};
