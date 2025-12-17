using UnityEngine;

namespace Player
{
    public class PlayerAimState : PlayerStateBase
    {
        public static readonly int isAim = Animator.StringToHash("isAiming");
        private static readonly int InputX = Animator.StringToHash("InputX");
        private static readonly int InputY = Animator.StringToHash("InputY");

        public PlayerAimState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            player.animator.SetBool(isAim, true);
            
            player.SetAimCamera(true);
        }

        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                player.Shoot();
                return;
            }

            if (Input.GetMouseButtonUp(1))
            {
                stateMachine.ChangeState(player.GetState<PlayerIdleState>());
                return;
            }
            HandleMovement();
        }
        
        private void HandleMovement()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            
            if (Mathf.Abs(x) < 0.1f && Mathf.Abs(z) < 0.1f)
            {
                player.animator.SetFloat(InputX, 0, 0.1f, Time.deltaTime);
                player.animator.SetFloat(InputY, 0, 0.1f, Time.deltaTime);
                return;
            }
            
            Vector3 moveDirection = (player.transform.forward * z + player.transform.right * x).normalized;
            
            Vector3 velocity = (moveDirection * player.aimMoveSpeed) + player.GetVerticalVector();
            
            player.characterController.Move(velocity * Time.deltaTime);
            
            player.animator.SetFloat(InputX, x, 0.1f, Time.deltaTime);
            player.animator.SetFloat(InputY, z, 0.1f, Time.deltaTime);
        }

        public override void Exit()
        {
            player.animator.SetBool(isAim, false);
            player.SetAimCamera(false);
            
            player.animator.SetFloat(InputX, 0, 0.1f, Time.deltaTime);
            player.animator.SetFloat(InputY, 0, 0.1f, Time.deltaTime);
            
            // TODO : 카메라 줌 아웃, 조준 UI 비활성화 등
        }
    }
}