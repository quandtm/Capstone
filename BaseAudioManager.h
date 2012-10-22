#pragma once
#include <map>
#include "AudioTrack.h"

class BaseAudioManager
{
private:
	std::map<char*, AudioTrack> *_tracks;

public:
	BaseAudioManager(void);
	~BaseAudioManager(void);
};
