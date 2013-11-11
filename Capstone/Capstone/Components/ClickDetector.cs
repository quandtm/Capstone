using System.Collections.Generic;
using Capstone.Core;
using SharpDX;

namespace Capstone.Components
{
    public static class ClickDetector
    {
        private static readonly List<ClickArea> _areas;

        static ClickDetector()
        {
            _areas = new List<ClickArea>();
        }

        internal static void AddArea(ClickArea area)
        {
            _areas.Add(area);
        }

        internal static void RemoveArea(ClickArea area)
        {
            _areas.Remove(area);
        }

        public static Entity GetClicked(float x, float y)
        {
            for (int i = 0; i < _areas.Count; i++)
            {
                var c = _areas[i];
                if (c.ContainsPoint(x, y))
                    return c.Owner;
            }
            return null;
        }

        public static Entity FindIntersects(RectangleF rect)
        {
            for (int i = 0; i < _areas.Count; i++)
            {
                var c = _areas[i];
                if (c.Intersects(rect))
                    return c.Owner;
            }
            return null;
        }
    }
}
