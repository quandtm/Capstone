using Capstone.Core;
using Capstone.Editor.Common;
using Capstone.Editor.Scripts;
using Capstone.Engine.Scripting;

namespace Capstone.Editor.ViewModels
{
    public class EditorViewModel : BindableBase
    {
        private Entity _previewEntity;

        public EditorViewModel()
        {
            _previewEntity = new Entity();
            var script = new PointerFollowScript();
            ScriptManager.Instance.RegisterScript(script);
            _previewEntity.AddComponent("ptrScript", script);
        }
    }
}
