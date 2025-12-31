using System.Collections;
using UnityEngine;

namespace Room
{
    public class DoorController : MonoBehaviour
    {
        [Header("연결된 방 정보")] 
        [SerializeField] private RoomController RoomA;
        [SerializeField] private RoomController RoomB;

        [Header("문 세팅")]
        [SerializeField] private DoorComponent[] doors;
        private readonly float openAngle = 90f;
        private readonly float closeAngle = 0f;
        
        private bool isOpen = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            if(isOpen) return;
            
            
            Vector3 Location = transform.position - other.transform.position;
            float dot = Vector3.Dot(Location, transform.up);
            Open(dot < 0f);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            if(!isOpen) return;

            isOpen = false;

            foreach (var door in doors)
            {
                door.RotateDoor(closeAngle);
            }
        }

        private void Open(bool IsBack)
        {
            isOpen = true;
            RoomManager.Instance.EnterRoom(IsBack ? RoomB : RoomA);

            foreach (var door in doors)
            {
                door.RotateDoor(IsBack ? -openAngle : openAngle);
            }
        }




    }
}