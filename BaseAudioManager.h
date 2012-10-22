#pragma once
#include <map>
#include "AudioTrack.h"

class BaseAudioManager
{
private:
	std::map<char*, AudioTrack> *_musicTracks;
	std::map<char*, AudioTrack> *_sfxTracks;

public:
	BaseAudioManager(void);
	~BaseAudioManager(void);
};
