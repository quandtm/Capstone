using Capstone.Editor.Common;

namespace Capstone.Editor.Data
{
    public abstract class ComponentTemplate : BindableBase
    {
        public string TemplateName { get; private set; }
        public string InstanceName { get; set; }

        public ComponentTemplate(string name)
        {
            TemplateName = name;
            InstanceName = string.Empty;
        }

        public virtual ComponentTemplate Clone()
        {
            return null;
        }
    }
}
