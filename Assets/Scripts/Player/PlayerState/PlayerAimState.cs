using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerAimState : PlayerStateBase
    {
        public static readonly int isAim = Animator.StringToHash("isAiming");
        private static readonly int InputX = Animator.StringToHash("InputX");
        private static readonly int InputY = Animator.StringToHash("InputY");
        private Transform cameraTransform;
        
        private Coroutine rotateCoroutine;

        public PlayerAimState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
            if (Camera.main is not null) cameraTransform = Camera.main.transform;
        }

        public override void Enter()
        {
            player.animator.SetBool(isAim, true);
            //TODO : player 애니메이터 설정
            
            if (rotateCoroutine is not null) player.StopCoroutine(rotateCoroutine);
            rotateCoroutine = player.StartCoroutine(RotateCameraForward());
            
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
                player.animator.SetBool(isAim, false);
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

            Vector3 velocity = (moveDirection * player.GetStatus(PlayerStatusType.aimMoveSpeed)) + player.GetVerticalVector();
            
            player.characterController.Move(velocity * Time.deltaTime);
            
            if (moveDirection.magnitude >= 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                player.transform.rotation = Quaternion.Slerp(
                    player.transform.rotation, 
                    targetRotation, 
                    Time.deltaTime * player.GetStatus(PlayerStatusType.aimTurnSpeed));
            }
            
            player.animator.SetFloat(InputX, x, 0.1f, Time.deltaTime);
            player.animator.SetFloat(InputY, z, 0.1f, Time.deltaTime);
        }

        private IEnumerator RotateCameraForward()
        {
            if (cameraTransform == null) yield break;

            // 목표 회전값 계산 (y축만 고려)
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0;
            if (camForward == Vector3.zero) camForward = player.transform.forward; // 예외 처리

            Quaternion startRotation = player.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(camForward);

            float time = 0f;
            float duration = 0.2f; // 회전하는 데 걸리는 시간 (빠르게 정렬)

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                
                // 부드러운 회전 적용
                player.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            // 확실하게 끝 맞춤
            player.transform.rotation = targetRotation;
            rotateCoroutine = null;

        }

        public override void Exit()
        {
            player.SetAimCamera(false);
            
            player.animator.SetFloat(InputX, 0, 0.1f, Time.deltaTime);
            player.animator.SetFloat(InputY, 0, 0.1f, Time.deltaTime);
        }
    }
}