using System;
using UnityEngine;

namespace BulletHell
{
    [CreateAssetMenu(menuName = "Bullet Hell/Color")]
    public class ColorInfo : ScriptableObject
    {
        public ColorData[] data;
    }

    // Future use: extend number of fields to include textures, materials, etc.
    [Serializable]
    public struct ColorData
    {
        public Color color;
    }
}
