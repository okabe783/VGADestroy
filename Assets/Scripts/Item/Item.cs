using VGADestroy.Character;
using VGADestroy.Common;

namespace VGADestroy.Item
{
    // Speedアイテム用のクラス
    public class Item : ItemBase
    {
        public override void Apply(PlayerStatus playerStatus)
        {
            // PlayerStatusに効果を渡す
            //playerStatus.AddSpeed();
        }
    }
}