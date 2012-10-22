#include "pch.h"
#include "BaseAudioManager.h"

BaseAudioManager::BaseAudioManager(void)
{
	_musicTracks = new std::map<char*, AudioTrack>();
	_sfxTracks = new std::map<char*, AudioTrack>();
}

BaseAudioManager::~BaseAudioManager(void)
{
	delete _musicTracks;
	delete _sfxTracks;
}
