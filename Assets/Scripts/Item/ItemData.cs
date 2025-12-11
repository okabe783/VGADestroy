using System;
using System.Collections.Generic;

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
    public class ItemData
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