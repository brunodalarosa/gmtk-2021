using UnityEngine;

namespace GMTK2021
{
    public class ColorBlock : Block
    {
        protected override void Action()
        {
            PlayerBlock.SpriteRenderer.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}