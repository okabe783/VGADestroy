using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VGADestroy.Item
{
    // ObjectPoolの実態
    public class ObjectPool<T> where T : PoolableObject
    {
        private readonly T[] _prefab;
        private readonly Transform _parent;
        private readonly Queue<T> _pool = new();
        
        // 現在Activeの数を計算する変数
        public int ActiveCount { get; private set; }

        // プレハブと親を設定して初期化
        public ObjectPool(T[] prefab, int initialCount, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            
            // 初期生成
            for (int i = 0; i < initialCount; i++)
            {
                Create();
            }
        }

        // イベントの購読、instanceの生成
        private T Create()
        {
            foreach (T t in _prefab)
            {
                T obj = Object.Instantiate(t);
                obj.gameObject.SetActive(false);
                obj.OnReturnedToPool = ReturnObject;
                _pool.Enqueue(obj);
                return obj;
            }

            return null;
        }

        // オブジェクトのアクティブ化
        // 超えてしまった場合の追加実行
        public T GetObject()
        {
            foreach (T obj in _pool.Where(obj => !obj.gameObject.activeSelf))
            {
                obj.gameObject.SetActive(true);
                ActiveCount++;
                return obj;
            }
            
            // 新規生成
            T newObj = Create();
            newObj.gameObject.SetActive(true);
            ActiveCount++;
            return newObj;
        }

        // 使用し終わったオブジェクトをプールに戻す
        private void ReturnObject(PoolableObject obj)
        {
            _pool.Enqueue((T)obj);
            ActiveCount--;
        }
    }
}