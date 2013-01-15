﻿using Capstone.Core;
using Capstone.Editor.Common;
using Capstone.Editor.Data;
using Capstone.Editor.Scripts;
using Capstone.Engine.Graphics;
using Capstone.Engine.Scripting;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Windows.Storage;

namespace Capstone.Editor.ViewModels
{
    public class EditorViewModel : BindableBase
    {
        private Entity _previewEntity;

        public ObservableCollection<SpritePreview> Sprites { get; private set; }
        private SpritePreview _selected;
        public SpritePreview SelectedSprite
        {
            get { return _selected; }
            set
            {
                if (_selected == value || _previewEntity == null) return;
                _selected = value;

                if (_selected != null)
                {
                    var old = (Texture)_previewEntity.GetComponent("sprite");
                    if (old != null)
                    {
                        SpriteRenderer.Instance.RemoveTexture(old);
                        _previewEntity.RemoveComponent("sprite");
                    }

                    var tex = new Texture(_selected.FilePath);
                    SpriteRenderer.Instance.RegisterTexture(tex);
                    _previewEntity.AddComponent("sprite", tex);
                }
                else
                {
                    var tex = (Texture)_previewEntity.GetComponent("sprite");
                    if (tex != null)
                        tex.IsVisible = false;
                }
            }
        }

        public EditorViewModel()
        {
            Sprites = new ObservableCollection<SpritePreview>();

            _previewEntity = new Entity();
            var script = new PointerFollowScript();
            ScriptManager.Instance.RegisterScript(script);
            _previewEntity.AddComponent("ptrScript", script);

            SelectedSprite = null;
        }

        public async void OpenSprite(StorageFile file)
        {
            var path = file.Path;
            if (!Path.GetExtension(path).Equals(".dds", StringComparison.CurrentCultureIgnoreCase))
                throw new ArgumentException("File must be a DDS.");

            var local = ApplicationData.Current.LocalFolder;
            var cpy = await file.CopyAsync(local, Path.GetFileName(path), NameCollisionOption.ReplaceExisting);

            path = cpy.Path;
            var sprite = SpritePreview.Load(path);
            Sprites.Add(sprite);
            Capstone.Engine.Graphics.SpriteRenderer.Instance.Preload(path);
        }
    }
}
