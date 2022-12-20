using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.portfolio.interfaces;

namespace com.portfolio.player
{

    [RequireComponent(typeof(CharacterController))]
    public partial class Player : MonoBehaviour
    {

        private CachedComponents cachedComponents;
        private CharacterController CharacterController
        {
            get
            {
                if (cachedComponents is null)
                    cachedComponents = new(this);
                return cachedComponents.CharacterController;
            }
        }

        private Player Context=>this;
        private Player.State CurrentState { get; set; }

        private Player.State WalkingState => new Player.State.Walking();
        private Player.State JumpState => new Player.State.Jump();
        private Player.State RunningState => new Player.State.Running();
        private Player.State IdleState => new Player.State.Idle();

        private PlayerConfig PlayerConfig { get; set; }

        private Vector2 MoveInput
        {
            get;
            set;
        }
        private bool JumpInput { get; set; }
        private bool RunInput { get; set; }

        private void Start()
        {
            CurrentState = IdleState;

            CurrentState.OnEnterState(Context);
        }

        private void Update()
        {
            Debug.Log(CurrentState);
            CurrentState.OnUpdateState(Context);
        }

        private void Move(float speed)
        {
            CharacterController.Move(speed * Time.deltaTime * new Vector3(MoveInput.x, 0, MoveInput.y)); ;
        }

    }
}

