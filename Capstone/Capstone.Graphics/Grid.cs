using SharpDX;

namespace Capstone.Graphics
{
    public class Grid
    {
        private Vector3 _topLeftCenter;
        private Vector3 _topLeft;
        private float _cellSize;

        public Grid(Vector3 topLeft, float cellSize)
        {
            _cellSize = cellSize;
            _topLeft = topLeft - (new Vector3(.5f, .5f, 0) * cellSize);
            _topLeftCenter = _topLeft + new Vector3(cellSize / 2, cellSize / 2, 0);
        }

        public Vector3 ToCellCenter(int x, int y, float? cellSize = null)
        {
            var cs = cellSize.HasValue ? cellSize.Value : _cellSize;
            return _topLeftCenter + (new Vector3(x, y, 0) * cs);
        }

        public void ToCellCoords(Vector3 v, out int x, out int y, float? cellSize = null)
        {
            var cs = cellSize.HasValue ? cellSize.Value : _cellSize;
            var dir = v - _topLeft;
            x = (int)(dir.X / cs);
            y = (int)(dir.Y / cs);
        }

        public Vector3 Snap(Vector3 v, float? altCellSize = null)
        {
            int x, y;
            ToCellCoords(v, out x, out y, altCellSize);
            return ToCellCenter(x, y, altCellSize);
        }
    }
}
