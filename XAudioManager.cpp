#include "pch.h"
#include "XAudioManager.h"

XAudioManager::XAudioManager(void)
{
	_musicReady = false;
	_sfxReady = false;
}

XAudioManager::~XAudioManager(void)
{
}

void XAudioManager::Initialise()
{
	if (FAILED(XAudio2Create(&_musicEngine)))
		return;

	if (FAILED(_musicEngine->CreateMasteringVoice(&_musicVoice)))
		return;

	_musicReady = true;

	if (FAILED(XAudio2Create(&_sfxEngine)))
		return;

	if (FAILED(_sfxEngine->CreateMasteringVoice(&_sfxVoice)))
		return;

	_sfxReady = true;
}
