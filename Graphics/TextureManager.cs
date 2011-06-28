using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine.Graphics;

namespace SmallGalaxy_Engine
{

    public class TextureSet : IDisposable
    {

        #region Fields

        private string _name;
        private ContentManager _content;
        private Dictionary<string, Texture2D> _textures = new Dictionary<string,Texture2D>();

        #endregion // Fields


        #region Init

        public TextureSet(string name, ContentManager content)
        {
            _name = name;
            _content = content;
        }

        public void Dispose()
        {
            _textures.Clear();
        }

        #endregion // Init


        #region Methods

        public void AddTexture(string name, string filename)
        {
            var texture = _content.Load<Texture2D>(filename);
            AddTexture(name, texture);
        }
        private void AddTexture(string name, Texture2D texture)
        {
            if (_textures.ContainsKey(name))
            {
                if (_textures[name] != texture)
                {
                    // A Different Texture has been loaded with that same name...
                    throw new InvalidOperationException(string.Format("Texture '{0}' has already been loaded", name));
                }
                else
                {
                    // Were just re-loading the same texture... so do nothing
                    return;
                }
            }

            _textures.Add(name, texture);
        }
        public Texture2D GetTexture(string name)
        {
            Texture2D tex;
            if (!_textures.TryGetValue(name, out tex))
            {
                throw new ArgumentException(string.Format("Texture '{0}' not found", name));
            }
            return tex;
        }

        #endregion // Methods

    }

    public class TextureManager
    {
        // a singleton of the texture sets loaded by the game
        private static Dictionary<string, TextureSet> _textureSets = new Dictionary<string, TextureSet>();

        #region Fields

        private string _name;
        private ContentManager _content;

        #endregion // Fields


        #region Properties

        public ContentManager ContentManager { get { return _content; } set { SetContentManager(value); } }

        #endregion // Properties


        #region Init

        public TextureManager(string sceneName)
        {
            _name = sceneName;
        }

        public static TextureManager GetInstance(string sceneName)
        {
            return new TextureManager(sceneName);            
        }

        #endregion // Init


        #region Methods

        public void AddTexture(string name, string filename)
        {
            _textureSets[_name].AddTexture(name, filename);
        }
        public Texture2D GetTexture(string name)
        {
            return _textureSets[_name].GetTexture(name);
        }

        private void SetContentManager(ContentManager content)
        {
            if (_content != content)
            {
                _content = content;
                TextureSet ts = new TextureSet(_name, _content);

                // Changing Content Manager means Disposing the old Texture Set for the screen 
                if (_textureSets.ContainsKey(_name)) { _textureSets[_name].Dispose(); _textureSets.Remove(_name); }

                _textureSets.Add(_name, ts);
            }
        }

        #endregion // Methods

    }
}
