using System.Collections.Generic;
using Command;
using UnityEngine;

namespace Level
{
    public class BlocksInputHandler : MonoBehaviour
    {
        public float _walkingSpeed = 15f;
        
        public List<BaseCommand> Commands { get; private set; }

        private void Start()
        {
            Commands = new List<BaseCommand>();
        }

        public void ReadInputs()
        {
            var movementInput = Input.GetAxisRaw("Horizontal") * _walkingSpeed;
            if (movementInput != 0) Commands.Add(new WalkCommand(movementInput));

            if (Input.GetKeyDown(KeyCode.Space)) Commands.Add(new JumpCommand());
            if (Input.GetKeyDown(KeyCode.J)) Commands.Add(new DashCommand());
            if (Input.GetKeyDown(KeyCode.K)) Commands.Add(new ShootCommand());
        }
    }
}