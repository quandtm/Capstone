#pragma once

class IGame
{
public:
	virtual ~IGame() {};
	virtual void Initialise() = 0;
	virtual void Load() = 0;
	virtual void Update(double elapsedSeconds) = 0;
};
