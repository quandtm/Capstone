﻿using System;
using SharpDX;

namespace Capstone.Core
{
    public sealed class Transform2D
    {
        private readonly Entity _entity;
        private Vector3 _translation;
        private Vector2 _scale;
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

        public Vector3 LocalTranslation
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

        #region World Transformations
        public Vector3 Translation
        {
            get
            {
                Matrix m = Matrix.Identity;
                if (_entity.Parent != null)
                    _entity.Parent.Transform.GetWorld(out m);
                return Vector3.TransformCoordinate(_translation, m);
            }
        }

        public Vector2 Scale
        {
            get
            {
                Vector2 s = Vector2.One;
                if (_entity.Parent != null)
                    s = _entity.Parent.Transform.Scale;
                return s * _scale;
            }
        }

        public float Rotation
        {
            get
            {
                float r = 0;
                if (_entity.Parent != null)
                    r = _entity.Parent.Transform.Rotation;
                return r + _rotation;
            }
        }
        #endregion

        public Transform2D(Entity e)
        {
            if (e == null) throw new NullReferenceException("Entity cannot be null");
            _entity = e;
            _translation = Vector3.Zero;
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
            Matrix.Translation(_translation.X, _translation.Y, _translation.Z, out t);
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
