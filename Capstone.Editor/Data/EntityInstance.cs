﻿using Capstone.Core;
using Capstone.Editor.Common;

namespace Capstone.Editor.Data
{
    public class EntityInstance : BindableBase
    {
        public Entity Entity { get; private set; }

        private EntityInstance()
        {
        }

        public static EntityInstance Create(EntityTemplate template)
        {
            var instance = new EntityInstance();
            instance.Entity = template.BuildAndSetupEntity();
            return instance;
        }
    }
}