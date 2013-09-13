using System;
using SharpDX;

namespace Capstone.Core
{
    public sealed class Transform2D
    {
        private Entity _entity;
        private Vector2 _translation, _scale;
        private float _rotation;
        private Matrix _world;
        private bool _dirty;

        #region Local Transformations
        public float LocalRotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation == value) return;
                _rotation = value;
                _dirty = true;
            }
        }

        public Vector2 LocalTranslation
        {
            get { return _translation; }
            set
            {
                if (_translation == value) return;
                _translation = value;
                _dirty = true;
            }
        }

        public Vector2 LocalScale
        {
            get { return _scale; }
            set
            {
                if (_scale == value) return;
                _scale = value;
                _dirty = true;
            }
        }
        #endregion

        public Transform2D(Entity e)
        {
            if (e == null) throw new NullReferenceException("Entity cannot be null");
            _entity = e;
            _translation = Vector2.Zero;
            _scale = Vector2.One;
            _rotation = 0;
            _world = Matrix.Identity;
            _dirty = false;
        }

        internal void UpdateWorld()
        {
            Matrix parent = Matrix.Identity;
            if (_entity.Parent != null)
                _entity.Parent.Transform.GetWorld(out parent);

            Matrix s, r, t, sr, srt;
            Matrix.Scaling(_scale.X, _scale.Y, 0, out s);
            Matrix.RotationZ(_rotation, out r);
            Matrix.Translation(_translation.X, _translation.Y, 0, out t);
            Matrix.Multiply(ref s, ref r, out sr);
            Matrix.Multiply(ref sr, ref t, out srt);
            Matrix.Multiply(ref srt, ref parent, out _world);

            _dirty = false;
            _entity.UpdateChildTransforms();
        }

        public void GetWorld(out Matrix worldMatrix)
        {
            if (_dirty)
                UpdateWorld();
            worldMatrix = _world;
        }
    }
}
