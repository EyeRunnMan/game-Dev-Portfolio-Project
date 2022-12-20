using com.portfolio.interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.portfolio.player
{
    public class TestPlayer : MonoBehaviour
    {
        public PlayerInvoker player;
        public PlayerInput playerInputs;

        private void Awake()
        {
            playerInputs = new();

            playerInputs.PlayerControls.Move.started += ExecuteMove;
            playerInputs.PlayerControls.Move.performed += ExecuteMove;
            playerInputs.PlayerControls.Move.canceled += ExecuteMove;


            playerInputs.PlayerControls.Running.started += ExecuteRun;
            playerInputs.PlayerControls.Running.performed += ExecuteRun;
            playerInputs.PlayerControls.Running.canceled += ExecuteRun;

        }

        private void OnEnable()
        {
            playerInputs.PlayerControls.Enable();
        }

        private void OnDisable()
        {
            playerInputs.PlayerControls.Disable();
        }

        public void ExecuteMove(InputAction.CallbackContext ctx)
        {
            ICommand<Player> movePlayerCommand = new Player.Commands.Move(ctx.ReadValue<Vector2>());
            player.ExecuteCommand(movePlayerCommand);
        }
        public void ExecuteJump(InputAction.CallbackContext ctx)
        {
            ICommand<Player> jumpPlayerCommand = new Player.Commands.Jump();
            player.ExecuteCommand(jumpPlayerCommand);
        }
        public void ExecuteRun(InputAction.CallbackContext ctx)
        {
            ICommand<Player> runPlayerCommand = new Player.Commands.Run(ctx.ReadValueAsButton());
            player.ExecuteCommand(runPlayerCommand);
        }
    }
}

