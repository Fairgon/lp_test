using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Game
{
    [Serializable]
    public class CursorPreset
    {
        public CursorTypes Type => _type;
        [SerializeField]
        private CursorTypes _type = CursorTypes.Default;
        public Texture2D Texture => _texture;
        [SerializeField]
        private Texture2D _texture;
    }

    [Serializable, CreateAssetMenu(menuName = "Game/New Cursors Data")]
    public class CursorsData : ScriptableObject
    {
        [SerializeField]
        private List<CursorPreset> presets = null;

        public CursorPreset GetCursor(CursorTypes type)
        {
            return presets.FirstOrDefault(x => x.Type == type);
        }
    }
}