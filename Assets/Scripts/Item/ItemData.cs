using System;
using System.Collections.Generic;
using UnityEngine;

namespace VGADestroy.Item
{
    public enum ItemType
    {
        None,
        SpeedUp,
        SpeedDown,
        PowerUp,
        PowerDown,
    }
    
    // Itemのパラメーターを設定する
    [CreateAssetMenu(fileName = "Item", menuName = "ItemData", order = 0)]
    public class ItemData : ScriptableObject
    {
        public List<ItemDataList>  Items;
        
        [Serializable]
        public class ItemDataList
        {
            public ItemType ItemType;
            
            public float Speed;
            public float Power;
        }
    }
}