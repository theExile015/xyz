using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;

        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
           return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }

        internal void Get()
        {
            foreach (var item in _items)
            {
                item.
            }
        }
    }
}