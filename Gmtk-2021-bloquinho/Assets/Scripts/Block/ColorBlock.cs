using UnityEngine;

namespace Block
{
    public class ColorBlock : BaseBlock
    {
        protected override void Action()
        {
            PlayerBlock.SpriteRenderer.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}