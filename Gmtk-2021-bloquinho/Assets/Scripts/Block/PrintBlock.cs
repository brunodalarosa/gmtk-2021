using UnityEngine;

namespace GMTK2021
{
    public class PrintBlock : Block
    {
        protected override void Action()
        {
            Debug.Log($"Printando porque essa é a minha função e meu nome é: {gameObject.name}");
        }
    }
}