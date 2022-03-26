/*
 *  GFXManager static class
 *  =======================
 *  
 *  Handles loading/unloading of spritesheets and
 *  raw Texture2D objects used by other classes.
 *  Uses a pair of Dictionaries to organize spritesheets
 *  and track how many existing objects have that particular
 *  spritesheet loaded, unloading when no objects are
 *  using it any longer.
 * 
 */

using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace RavenLib.Graphics
{
    public class LoadedTexture
    {
        public int ConnectedObjects;    // Number of objects utilizing the referenced Texture2D object
        public Texture2D Texture;       // Reference to the loaded Texture2D object
    }

    internal static class GFXManager
    {
        private static ContentManager gfxContent;   // Reference to the MainGame's ContentManager. Gives us a hook into the primary Content directory
        private static Dictionary<string, string> _textureMap;  // A key-value relationship between spritesheet ID's and their affiliated Content paths
        private static Dictionary<string, LoadedTexture> _loadedTextures;   // A key-value relationship between spritesheet ID's and the number of objects using the loaded texture
        private static string _gfxJsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Night Owl Software\\RPGGame\\data\\TextureMap.json");  // The TextureMap.json file contains the information stored in _textureMap's dictionary

        /// <summary>
        /// Connect the ContentManager to the GFXManager and create associated texture maps
        /// </summary>
        /// <param name="content">MainGame ContentManager</param>
        public static void Initialize(ContentManager content)
        {
            gfxContent = content;
            _loadedTextures = new Dictionary<string, LoadedTexture>(); // By default, no textures are loaded, so this can be initialized as empty
            InitializeSpritesheetMap();
        }

        /// <summary>
        /// Deserialize the contents of the TextureMap.json to build the _textureMap Dictionary
        /// </summary>
        private static void InitializeSpritesheetMap()
        {
            _textureMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_gfxJsonPath));
        }

        /// <summary>
        /// Load the associated Texture2D object by SpritesheetID reference, or hook into an existing loaded object. Returns the associated Texture2D object.
        /// </summary>
        /// <param name="spritesheetId">The string ID of the spritesheet</param>
        /// <returns>Texture2D object</returns>
        public static Texture2D GetSpriteSheet(string spritesheetId)
        {
            // Check first if no textures are loaded yet or if the texture that is being asked for is not loaded
            // and if so, immediately load it and set the number of connected objects to 1
            if (_loadedTextures.Count == 0 || !_loadedTextures.ContainsKey(spritesheetId))
            {
                Texture2D _texture = gfxContent.Load<Texture2D>(_textureMap[spritesheetId]);
                LoadedTexture _loadedTexture = new LoadedTexture { Texture = _texture, ConnectedObjects = 1 };
                _loadedTextures.Add(spritesheetId, _loadedTexture);
                return _texture;
            }
            else  // Otherwise, simply increase the number of associated objects
            {
                _loadedTextures[spritesheetId].ConnectedObjects++;
            }

            // Once the previous steps are completed, the texture should be properly loaded into the
            // _loadedTextures map, so we can return it regardless.
            Texture2D spriteSheet = _loadedTextures[spritesheetId].Texture;
            return spriteSheet;
        }

        /// <summary>
        /// Unloads the spritesheet if no additional objects are using it. This needs to be called by any object using the GFXManager when it is unloaded or disposed from the MainGame.
        /// </summary>
        /// <param name="spritesheetId">String ID of the Spritesheet</param>
        public static void UnloadTexture(string spritesheetId)
        {
            // Check if _loadedTextures contains the spritesheet requested for unload or
            // the _loadedTextures map is empty / null. We do nothing in these cases
            if (_loadedTextures != null && _loadedTextures.ContainsKey(spritesheetId))
            {
                // If the requesting object is the only object still using
                // this Texture, we will proceed with the unload, otherwise
                // we simply decrement the number of connected objects
                if (_loadedTextures[spritesheetId].ConnectedObjects == 1)
                {
                    _loadedTextures[spritesheetId].Texture = null;
                    _loadedTextures[spritesheetId].Texture.Dispose();
                    _loadedTextures.Remove(spritesheetId);
                }
                else
                {
                    _loadedTextures[spritesheetId].ConnectedObjects--;
                }
            }
        }
    }
}
