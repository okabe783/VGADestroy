using System.Collections.Generic;
using UnityEngine;

namespace VGADestroy.Item.Poisson
{
    // アイテム生成アルゴリズム
    // Poisson Disk Sampling（2D XZ）
    // ・各点同士が必ず radius 以上離れるように配置できる
    // ・地面上のアイテム配置に最適
    public class Poisson2D
    {
        /// <summary>
        /// Poisson Disk Sampling（2D XZ）で点群を生成する
        /// </summary>
        /// <param name="radius">点同士の最小距離</param>
        /// <param name="regionSize">点を生成する領域（X,Z）サイズ</param>
        /// <param name="rejection">1つの点から生成候補を何回試すか（推奨:20〜40）</param>
        /// <returns>生成された点（Y=0）の List</returns>
        public static List<Vector3> GeneratePoisson2D(float radius, Vector2 regionSize, int rejection = 30)
        {
            // 2D 空間での最小セルの大きさ
            // 半径 ÷ √2（2次元分の分離距離）
            float cellSize = radius / Mathf.Sqrt(2);

            // 配列サイズ（切り上げ）
            int gx = Mathf.CeilToInt(regionSize.x / cellSize);
            int gz = Mathf.CeilToInt(regionSize.y / cellSize);

            // grid[x,z] には pointIndex+1 を入れる
            int[,] grid = new int[gx, gz];

            // 最終的に生成された点
            List<Vector3> points = new();
            // 新しい候補点生成の起点となる点
            List<Vector3> spawnPoints = new();

            // 最初の点を領域の中心に
            spawnPoints.Add(new Vector3(regionSize.x / 2, 0f, regionSize.y / 2));

            // 候補点が尽きるまで探索を続ける
            while (spawnPoints.Count > 0)
            {
                int spawnIndex = Random.Range(0, spawnPoints.Count);
                Vector3 spawnCentre = spawnPoints[spawnIndex];
                bool accepted = false;

                // rejection 回だけ候補点を生成して試す
                for (int i = 0; i < rejection; i++)
                {
                    float angle = Random.value * Mathf.PI * 2;

                    // 2Dの方向ベクトル（XZ）
                    Vector3 direction = new(
                        Mathf.Cos(angle),  // x
                        0f,
                        Mathf.Sin(angle)   // z
                    );

                    // radius〜2radius の範囲で距離を取る
                    Vector3 candidate = spawnCentre + direction * Random.Range(radius, radius * 2);

                    // 候補点が有効かどうかチェック
                    if (IsValid(candidate, regionSize, cellSize, radius, points, grid))
                    {
                        points.Add(candidate);
                        spawnPoints.Add(candidate);

                        int x = (int)(candidate.x / cellSize);
                        int z = (int)(candidate.z / cellSize);

                        grid[x, z] = points.Count;

                        accepted = true;
                        break;
                    }
                }

                // rejection 回失敗 → この起点からはもう生成できないので削除
                if (!accepted)
                {
                    spawnPoints.RemoveAt(spawnIndex);
                }
            }

            return points;
        }

        /// <summary>
        /// 候補点 candidate が「半径以内に他の点がいないか」を判定する
        /// </summary>
        private static bool IsValid(
            Vector3 candidate, Vector2 regionSize, float cellSize, float radius,
            List<Vector3> points, int[,] grid)
        {
            // 範囲外なら即 NG
            if (candidate.x < 0 || candidate.x >= regionSize.x ||
                candidate.z < 0 || candidate.z >= regionSize.y)
            {
                return false;
            }

            // grid 上のセル位置
            int x = (int)(candidate.x / cellSize);
            int z = (int)(candidate.z / cellSize);

            // 探索範囲（2セルでも十分だが安全に3セル見ておく）
            int searchRadius = 3;

            for (int ix = x - searchRadius; ix <= x + searchRadius; ix++)
            {
                for (int iz = z - searchRadius; iz <= z + searchRadius; iz++)
                {
                    // grid 範囲チェック
                    if (ix < 0 || ix >= grid.GetLength(0) ||
                        iz < 0 || iz >= grid.GetLength(1)) continue;

                    int pointIndex = grid[ix, iz] - 1;
                    if (pointIndex == -1) continue;

                    // XZ距離のみ使用
                    float dist = Vector2.Distance(
                        new Vector2(candidate.x, candidate.z),
                        new Vector2(points[pointIndex].x, points[pointIndex].z)
                    );

                    if (dist < radius)
                        return false;
                }
            }

            return true;
        }
    }
}
