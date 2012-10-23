#include "pch.h"

char* GuidToStr(LGUID guid)
{
	auto str = new char[17];
	memcpy(str, &guid, sizeof(char) * 16);
	str[16] = '\0';
	return str;
}

LGUID StrToGuid(char *str)
{
	LGUID result = {0};
	if (strlen(str) <= 16)
		memcpy(&result, str, sizeof(char) * 16);
	return result;
}
