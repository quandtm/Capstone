#pragma once

class Resources
{
private:
	static Resources *_instance;
	Resources(void);

public:
	~Resources(void);

	void GetResource(LGUID typeId, LGUID itemId);

	static Resources* GetInstance(void)
	{
		if (_instance == nullptr)
			_instance = new Resources();
		return _instance;
	}
};
