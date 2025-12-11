using UnityEngine;
using VGADestroy.Character;
using VGADestroy.Item;

namespace VGADestroy.Common
{
    // アイテムの基底クラス
    public abstract class ItemBase : PoolableObject
    {
        public ItemData.ItemDataList DataSO;

        public void OnCollisionEnter(Collision other)
        {
            // Playerなら消す
            if(!other.gameObject.CompareTag("Player"))
                return;
            
            Debug.Log("Hit: " + other.gameObject.name);
            Apply(other.gameObject.GetComponent<PlayerStatus>());
            // Objectがなくなるときにこのオブジェクトを返却する
            ReturnToPool();
        }

        public abstract void Apply(PlayerStatus playerStatus);
    }
}