using SharpDX;

namespace Capstone.Graphics
{
    public class Grid
    {
        private Vector3 _topLeftCenter;
        private Vector3 _topLeft;
        private float _cellSize;
        private int _width;
        private int _height;

        public Grid(Vector3 center, float cellSize, int width, int height)
        {
            _cellSize = cellSize;
            _width = width;
            _height = height;
            _topLeft = center - (new Vector3(width / 2, height / 2, 0) * cellSize);
            _topLeftCenter = _topLeft + new Vector3(cellSize / 2, cellSize / 2, 0);
        }

        public bool ContainsPoint(Vector3 v)
        {
            int x, y;
            ToCellCoords(v, out x, out y);
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }

        public Vector3 ToCellCenter(int x, int y)
        {
            return _topLeftCenter + (new Vector3(x, y, 0) * _cellSize);
        }

        public void ToCellCoords(Vector3 v, out int x, out int y)
        {
            x = y = 0;
            var dir = v - _topLeft;
            x = (int)(dir.X / _cellSize);
            y = (int)(dir.Y / _cellSize);
        }

        public Vector2 ToCellCoords(Vector3 v)
        {
            int x, y;
            ToCellCoords(v, out x, out y);
            return new Vector2(x, y);
        }

        public Vector3 Snap(Vector3 v)
        {
            int x, y;
            ToCellCoords(v, out x, out y);
            return ToCellCenter(x, y);
        }
    }
}
