using UnityEngine;
using VGADestroy.Character;
using VGADestroy.Item;

namespace VGADestroy.Common
{
    // アイテムの基底クラス
    public abstract class ItemBase : MonoBehaviour
    {
        protected ItemData.ItemDataList ItemData;
        public ItemData.ItemDataList DataSO;

        public void Initialize(ItemData.ItemDataList data)
        {
            ItemData = data;
        }
        
        private void OnValidate()
        {
            // エディタで設定されたら ItemData に反映
            if (DataSO.ItemType == ItemType.None)
            {
                ItemData = DataSO;
            }
        }
        
        public abstract void Apply(PlayerStatus playerStatus);

        public ItemData.ItemDataList Pick()
        {
            return ItemData;
        }
    }
}