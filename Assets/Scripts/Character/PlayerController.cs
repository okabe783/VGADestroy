using UnityEngine;
using VGADestroy.Common;

namespace VGADestroy.Character
{
    // 衝突判定などを管理する
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerStatus _playerStatus;
        
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        // 触れたアイテムの効果を適用して破棄する
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<ItemBase>() == null) return;
            ItemBase itemData = collision.gameObject.GetComponent<ItemBase>();
            // アイテムの効果を適用
            itemData.Apply(_playerStatus);
            // アイテム破棄
            Destroy(itemData.gameObject);
        }
    }
}