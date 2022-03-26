using System.Collections.Generic;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Tiled;

namespace RavenLib.Scenes
{
    public class SceneManager
    {
        private TiledMap _activeMap;
        private Dictionary<string, TiledMap> _connectedMaps;
        private OrthographicCamera _sceneCamera;

        public SceneManager(TiledMap activeMap, Dictionary<string, TiledMap> connectedMaps)
        {

        }
    }
}
