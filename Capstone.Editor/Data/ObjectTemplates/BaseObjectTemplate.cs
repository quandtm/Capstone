﻿using Capstone.Core;
using Capstone.Editor.Common;
using Capstone.Engine.Graphics;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Capstone.Editor.Data.ObjectTemplates
{
    public abstract class BaseObjectTemplate : BindableBase
    {
        public ImageSource PreviewImage { get; private set; }
        public string PreviewLabel { get; protected set; }

        protected string DDSPath { get; private set; }
        protected string PreviewPath { get; private set; }

        public int Cost { get; protected set; }

        public int MaxInstances { get; protected set; }
        public int InstancesUsed { get; protected set; }
        public int InstancesRemaining
        {
            get
            {
                if (MaxInstances == 0)
                    return 1;
                else
                    return MaxInstances - InstancesUsed;
            }
        }

        protected BaseObjectTemplate(int maxInstances, int costPerInstance = 1)
        {
            MaxInstances = maxInstances;
            InstancesUsed = 0;
            Cost = costPerInstance;
        }

        protected async Task LoadImages(string path)
        {
            path = path.Replace("/", "\\");
            var basePath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;

            var ddsPath = Path.Combine(basePath, path + ".dds");
            DDSPath = ddsPath;

            var previewPath = Path.Combine(basePath, path + "_preview.png");
            PreviewPath = previewPath;

            PreviewImage = new BitmapImage(new Uri(previewPath));
            SpriteRenderer.Instance.Preload(ddsPath);
        }

        public virtual Entity CreateEntityInstance()
        {
            if (InstancesRemaining <= 0) return null;
            var e = new Entity();
            var tex = new Texture(DDSPath);
            tex.Origin = OriginPoint.Center;
            e.AddComponent("sprite", tex);
            SpriteRenderer.Instance.RegisterTexture(tex);
            InstancesUsed = InstancesUsed + 1;

            return e;
        }
    }
}
