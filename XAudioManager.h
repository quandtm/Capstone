#pragma once
#include "BaseAudioManager.h"

class XAudioManager : public BaseAudioManager
{
private:
	bool _musicReady, _sfxReady;
	Microsoft::WRL::ComPtr<IXAudio2> _musicEngine;
	Microsoft::WRL::ComPtr<IXAudio2> _sfxEngine;
	IXAudio2MasteringVoice *_musicVoice;
	IXAudio2MasteringVoice *_sfxVoice;

public:
	XAudioManager(void);
	~XAudioManager(void);

	void Initialise();
	void Suspend();
	void Resume();
};
