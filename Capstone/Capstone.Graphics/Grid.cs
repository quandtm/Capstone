using SharpDX;

namespace Capstone.Graphics
{
    public static class Grid
    {
        public static Vector3 Snap(Vector3 v)
        {
            return new Vector3((int)v.X, (int)v.Y, (int)v.Z);
        }
    }
}
