#pragma once
class AudioTrack
{
public:
	AudioTrack(void);
	~AudioTrack(void);

	void Load(wchar_t *path, IXAudio2 *engine);

private:
	IXAudio2SourceVoice *_voice;
	char *_data;
	bool _ready;
};
