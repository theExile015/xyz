using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.Model.Player;
using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private ItemsRepository _items;
        [SerializeField] private ThrowableRepository _throwableItems;
        [SerializeField] private PotionRepository _potions;
        [SerializeField] private PlayerDef _player;
        [SerializeField] private PerksRepository _perks;

        public ItemsRepository Items => _items;
        public ThrowableRepository Throable => _throwableItems;
        public PotionRepository Potions => _potions;
        public PerksRepository Perks => _perks;
        public PlayerDef Player => _player;

        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
           return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }

    }
}