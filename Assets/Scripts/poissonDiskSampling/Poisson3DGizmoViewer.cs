using System.Collections.Generic;
using UnityEngine;

namespace VGADestroy.Item
{
    public class Poisson3DGizmoViewer : MonoBehaviour
    {
        // 点同士の最小距離
        [Header("Poisson Disk Sampling 設定")] public float radius = 2f; 
        public Vector3 regionSize = new(20, 20, 20);
        public int rejection = 30;

        // 表示する球の大きさ
        [Header("Gizmo 表示設定")] public float pointSize = 0.2f; 
        public Color regionColor = Color.yellow;
        public Color pointColor = Color.cyan;

        // 生成された点のキャッシュ
        private List<Vector3> points;

        // エディタで値が変更されたら自動再生成
        private void OnValidate()
        {
            Generate();
        }

        private void Generate()
        {
            points = Poisson.Poisson2D.GeneratePoisson2D(radius, regionSize, rejection);
        }

        private void OnDrawGizmos()
        {
            if (points == null) Generate();

            // ───────────────
            // 領域の境界ボックスを描画
            // ───────────────
            Gizmos.color = regionColor;

            // regionSize の中心位置は this.transform.position
            Vector3 center = transform.position + regionSize / 2f;
            Gizmos.DrawWireCube(center, regionSize);

            // ───────────────
            // Poisson 点を描画
            // ───────────────
            Gizmos.color = pointColor;

            foreach (var p in points)
            {
                Gizmos.DrawSphere(transform.position + p, pointSize);
            }
        }
    }
}