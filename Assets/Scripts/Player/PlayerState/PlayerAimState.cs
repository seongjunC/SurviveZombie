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
            
            RotateCamera(true);
            // TODO : 카메라 줌 인, 조준 UI 활성화 등
        }

        public override void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Shoot");
                player.Shoot();
                return;
            }

            if (Input.GetMouseButtonUp(1))
            {
                float x = Input.GetAxisRaw("Horizontal");
                float y = Input.GetAxisRaw("Vertical");

                if (Mathf.Abs(x) > 0.1f || Mathf.Abs(y) > 0.1f)
                {
                    stateMachine.ChangeState(player.GetState<PlayerMoveState>());
                }
                else
                {
                    stateMachine.ChangeState(player.GetState<PlayerIdleState>());
                }

                return;
            }
            RotateCamera(true);
            HandleMovement();
        }

        private void RotateCamera(bool ON)
        {
            player.mainCamera.gameObject.SetActive(!ON);
            player.aimCamera.gameObject.SetActive(ON);
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
            
            Vector3 Direction = new Vector3(x, 0, z).normalized;
            
            Quaternion targetRotation = Quaternion.LookRotation(Direction);
            player.transform.rotation = 
                Quaternion.Slerp(
                    player.transform.rotation, 
                    targetRotation, 
                    Time.deltaTime * player.turnSpeed);
            
            player.characterController.Move(player.aimMoveSpeed * Time.deltaTime * Direction);
            
            player.animator.SetFloat(InputX, x, 0.1f, Time.deltaTime);
            player.animator.SetFloat(InputY, z, 0.1f, Time.deltaTime);
        }

        public override void Exit()
        {
            player.animator.SetBool(isAim, false);
            
            RotateCamera(false);
            // TODO : 카메라 줌 아웃, 조준 UI 비활성화 등
        }
    }
}