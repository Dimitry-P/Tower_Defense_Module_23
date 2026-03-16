using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    [CreateAssetMenu]
    public sealed class UpgradeAsset : ScriptableObject
    {
        public Sprite sprite;

        public int[] costByLevel = {3};
    }
}