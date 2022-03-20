using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Player
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _inventorySize;
        [SerializeField] private int _maxHealth;
        [SerializeField] private StatDef[] _statas;
        public int InventorySize => _inventorySize;
        public StatDef[] Stats => _statas;
        public StatDef GetStat(StatId id) => _statas.FirstOrDefault(x => x.Id == id);
    }
}