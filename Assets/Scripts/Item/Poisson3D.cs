using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace VGADestroy.Item
{
    // アイテム生成アルゴリズム
    // Poisson Disk Sampling
    // 既存点の近くにランダムに候補点を作る　条件を満たせば採用　ダメなら繰り返す
    // 無限に繰り返すことはせずに自分で指定する
    // 繰り返す数が多ければ多いほど精度が落ちる
    public class Poisson3D
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="regionSize">点を生成する3Dの範囲</param>
        /// <param name="rejection">1つの地点から候補点をランダムに試す回数</param>
        /// <returns></returns>
        public static List<Vector3> GeneratePoisson3D(float radius, Vector3 regionSize, int rejection = 30)
        {
            // 点の数を1セルに最大1点しか生成できないようにする
            // 半径 ÷ 3次元(ルート3)
            float cellSize = radius / Mathf.Sqrt(3);
            // 3次元配列の作成
            // 端数切り上げ
            int [,,] grid = new int[
            Mathf.CeilToInt(regionSize.X / cellSize),
            Mathf.CeilToInt(regionSize.Y / cellSize), 
            Mathf.CeilToInt(regionSize.Z / cellSize)
            ];
            
            // 置くことが確定した点のリスト
            List<Vector3> points = new();
            // 新しい候補地を決めるために使う点のリスト
            List<Vector3> spawnPoints = new();
            
            // 生成の開始
            // 最初にランダムな点を置く
            spawnPoints.Add(regionSize / 2);

            // 候補の点がなくなるまで探索を続ける
            while (spawnPoints.Count > 0)
            {
                // 候補地をランダムに一つ選ぶ
                int spawnIndex = Random.Range(0, spawnPoints.Count);
                // この点を中心に候補店を生成する
                Vector3 spawnCentre = spawnPoints[spawnIndex];
                // 候補値が採用されたのかを判定するフラグ
                bool accepted = false;

                // 指定した回数分探索をする
                for (int i = 0; i < rejection; i++)
                {
                    float angle1 = Random.value * Mathf.PI * 2;
                    float angle2 = Random.value * Mathf.PI;
                }
            }
            return points;
        }
    }
}