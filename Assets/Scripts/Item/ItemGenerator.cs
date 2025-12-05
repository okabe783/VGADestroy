using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VGADestroy.Item.Poisson;
using Random = UnityEngine.Random;

namespace VGADestroy.Item
{
    // アイテムを生成するクラス
    // ObjectPool
    public class ItemGenerator : MonoBehaviour
    {
        [Header("Objectの生成に必要な設定"),SerializeField]
        private PoolableObject[] _itemPrefab;
        [Header("オブジェクトを再利用するためのPoolサイズ"),SerializeField]
        private int _poolSize = 96;
        [Header("Itemの生成数の調査時間"),SerializeField]
        private float _spawnInterval = 0.2f;
        [Header("アイテムの生成距離"), SerializeField]
        private float _minDistance = 1.0f;
        [SerializeField]
        private Vector3 _regionSize = new(50, 0, 50);
        
        private ObjectPool<PoolableObject> _pool;
        private CancellationTokenSource _cts;
        private List<Vector3> _spawnPoints;

        private void Start()
        {
            _cts = new CancellationTokenSource();
            _spawnPoints = Poisson2D.GeneratePoisson2D(_minDistance, new Vector2(_regionSize.x, _regionSize.z));
            _pool = new ObjectPool<PoolableObject>(_itemPrefab, initialCount: _poolSize / _itemPrefab.Length, transform.parent);
            MonitorLoop().Forget();
        }

        // 今Itemがいくつあるのかを調査して足りなければ再生成
        private async UniTaskVoid MonitorLoop()
        {
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_spawnInterval),cancellationToken: _cts.Token);
                    
                    // 不足数の計算
                    int need = Mathf.Max(0,_poolSize - _pool.ActiveCount);

                    if (need <= 0)
                    {
                        continue;
                    }
                    
                    for (int i = 0; i < need; i++)
                    {
                        CreateItem();
                    }
                }
            }
            catch (OperationCanceledException) { }
        }

        // Itemの生成処理
        private void CreateItem()
        {
            if (_spawnPoints == null || _spawnPoints.Count == 0)
            {
                Debug.LogError("ポイント地点がありません");
                return;
            }

            Vector3 localPos = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            localPos.y = 0f;

            PoolableObject obj = _pool.GetObject();
            obj.transform.position = transform.position + localPos;
        }
    }
}