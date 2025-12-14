using VGADestroy.Character;
using VGADestroy.Common;

namespace VGADestroy.Item
{
    // Speedアイテム用のクラス
    public class SpeedItem : ItemBase
    {
        protected override void Apply(PlayerStatus playerStatus)
        {
            // PlayerStatusに効果を渡す
            playerStatus.AddSpeed(DataSO.Speed);
        }
    }
}