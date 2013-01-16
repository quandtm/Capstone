namespace Capstone.Editor.Data.ObjectTemplates
{
    public class PlayerObject : BaseObjectTemplate
    {
        public PlayerObject()
            : base(1)
        {
            PreviewLabel = "Player";
            LoadImages("Content/sprites/player");
        }
    }
}
