using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Graphics
{
    public class FrameSet : IDisposable
    {

        #region Fields

        private string _name;
        private Dictionary<string, Frame> _frames;
        private Texture2D _texture;

        #endregion // Fields


        #region Properties

        public int Count { get { return _frames.Count; } }

        #endregion // Properties


        #region Init

        public FrameSet(string name, Texture2D texture)
        {
            _name = name;
            _texture = texture;
            _frames = new Dictionary<string, Frame>();
        }

        public void Dispose()
        {
            _frames.Clear();
            _texture = null;
        }

        #endregion // Init


        #region Methods

        public void AddFrame(string name, Rectangle sourceRectangle, SpriteEffects flipMode)
        {
            AddFrame(name, sourceRectangle, new Vector2(sourceRectangle.Width / 2f, sourceRectangle.Height / 2f), flipMode);
        }
        public void AddFrame(string name, Rectangle sourceRectangle)
        {
            AddFrame(name, sourceRectangle, new Vector2(sourceRectangle.Width / 2f, sourceRectangle.Height / 2f), SpriteEffects.None);
        }
        public void AddFrame(string name, Rectangle sourceRectangle, Vector2 center, SpriteEffects flipMode)
        {
            if (_frames.ContainsKey(name)) { throw new ArgumentException(string.Format("Frame Name: {0} Already Exists", name)); }
            _frames.Add(name, new Frame(_texture, sourceRectangle, center, flipMode));
        }

        public void SetFrame(string name, Rectangle sourceRectangle, SpriteEffects flipMode)
        {
            SetFrame(name, sourceRectangle, new Vector2(sourceRectangle.Width / 2f, sourceRectangle.Height / 2f), flipMode);
        }
        public void SetFrame(string name, Rectangle sourceRectangle)
        {
            SetFrame(name, sourceRectangle, new Vector2(sourceRectangle.Width / 2f, sourceRectangle.Height / 2f), SpriteEffects.None);
        }
        public void SetFrame(string name, Rectangle sourceRectangle, Vector2 center, SpriteEffects flipMode)
        {
            if (!_frames.ContainsKey(name)) { AddFrame(name, sourceRectangle, center, flipMode); }
            else
            {
                _frames[name].SourceRectangle = sourceRectangle;
                _frames[name].FlipMode = flipMode;
                _frames[name].Center = center;
            }
        }
        
        
        public Frame GetFrame(string name)
        {
            Frame result;
            _frames.TryGetValue(name, out result);
            return result;
        }
        public List<Frame> GetFrames()
        {
            return new List<Frame>(_frames.Values);
        }

        #endregion // Methods

    }
}
