using Block;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(InputHandler))]
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private BaseBlock _focusedBlock = null;
        private BaseBlock FocusedBlock => _focusedBlock;
        
        private InputHandler InputHandler { get; set; }

        private void Start()
        {
            InputHandler = GetComponent<InputHandler>();
            
            GameplayCamera.Instance.SetCamera(FocusedBlock.transform);
        }

        private void Update()
        {
            InputHandler.ReadInputs();
            
            if (InputHandler.Commands.Count > 0)
            {
                foreach (var command in InputHandler.Commands) command.Execute(FocusedBlock);
                InputHandler.Commands.Clear();
            }
            
            //necessário para o bloco que anda parar de andar depois de receber um input de andar
            if (FocusedBlock is PlayerBlock playerBlock)
                playerBlock.HandleMovement(0); 
        }
    }
}