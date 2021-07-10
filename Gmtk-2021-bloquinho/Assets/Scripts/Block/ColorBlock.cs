using UnityEngine;

namespace Block
{
    public class ColorBlock : BaseBlock
    {
        protected override void Action()
        {
            LeaderBlock.SpriteRenderer.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}