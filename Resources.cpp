#include "pch.h"
#include "Resources.h"

Resources::Resources(void)
{
}

Resources::~Resources(void)
{
}

// Resources should be stored in a 2 layer folder heirarchy
// First layer is the typeId as a string, then the itemId
// Per type handlers are registered beforehand

void Resources::GetResource(LGUID typeId, LGUID itemId)
{
}
