#include "pch.h"
#include "BaseAudioManager.h"

BaseAudioManager::BaseAudioManager(void)
{
	_tracks = new std::map<char*, AudioTrack>();
}

BaseAudioManager::~BaseAudioManager(void)
{
}
