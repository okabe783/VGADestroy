using System;
using UnityEngine;

namespace VGADestroy.Item
{
    public class PoolableObject : MonoBehaviour
    {
        // オブジェクトが非アクティブになった時にプールに戻すためのイベント
        public Action<PoolableObject> OnReturnedToPool;

        /// <summary>
        /// 使用後に戻す処理
        /// </summary>
        public void ReturnToPool()
        {
            OnReturnedToPool?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}