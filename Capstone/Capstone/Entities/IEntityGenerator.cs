using System.Collections.Generic;
using Capstone.Core;

namespace Capstone.Entities
{
    public interface IEntityGenerator
    {
        string TypeName { get; }
        Entity Generate(EntitySet set, string entityName, Dictionary<string, object> parameters);
    }
}
