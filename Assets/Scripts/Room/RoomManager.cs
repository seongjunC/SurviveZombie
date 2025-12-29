using System.Collections.Generic;
using System.Linq;
using pattern.Singleton;
using UnityEngine;

namespace Room
{
    public class RoomManager : Singleton<RoomManager>
    {
        private Dictionary<int, RoomController> roomsMap = new();

        private int _currentCenterRoomNum = 0;

        /// <summary>
        /// RoomController가 Awake시에 자신을 매니저에 등록합니다.
        /// </summary>
        public void RegisterRoom(RoomController room)
        {
            if (room is null) return;

            if (!roomsMap.TryAdd(room.roomNum, room)) return;

            // 현재 방의 번호가 중심방인 경우 모든 방을 갱신합니다. 
            if (room.roomNum == _currentCenterRoomNum)
            {
                RefreshRooms();
            }
            // 아닐경우 해당 방만 setActive할지 결정합니다
            else
            {
                UpdateRoomState(room);
            }
        }

        /// <summary>
        /// 씬이 전환되어 room이 파괴되었을때 사용합니다
        /// </summary>
        /// <param name="room"></param>
        public void UnRegisterRoom(RoomController room)
        {
            roomsMap.Remove(room.roomNum);
        }

        /// <summary>
        /// 특정 방에 진입시에 해당 방을 중심 방으로 전환하고 연결된 방을 갱신합니다. 
        /// </summary>
        /// <param name="centerRoom"> 활성화할 중심 방 </param>
        public void EnterRoom(RoomController centerRoom)
        {
            if (centerRoom is null) return;
            
            // 중앙 방의 번호를 저장합니다.
            _currentCenterRoomNum = centerRoom.roomNum;
            // 모든 방을 갱신합니다.
            RefreshRooms();
        }

        /// <summary>
        /// 연결된 방을 찾아서 해당 방의 상태를 업데이트 합니다.
        /// </summary>
        public void RefreshRooms()
        {
            foreach (var room in roomsMap.Values)
            {
                UpdateRoomState(room);
            }
        }
        
        /// <summary>
        /// 해당하는 방의 상태를 업데이트합니다.
        /// 해당하는 방이 현재 방이거나, 연결된 방이라면 활성화합니다.
        /// </summary>
        /// <param name="room"></param>
        public void UpdateRoomState(RoomController room)
        {
            roomsMap.TryGetValue(_currentCenterRoomNum, out var targetRoom);

            bool isActivate = false;

            if (targetRoom is not null)
            {
                isActivate = (room.roomNum == _currentCenterRoomNum) ||
                             targetRoom.connectedRoomNums.Contains(room.roomNum); 
            }
            room.gameObject.SetActive(isActivate);
            
            if(room.gameObject.activeSelf) room.SpawnMonsters();
            else room.DespawnMonsters();
        }

        public void Initialize()
        {
            //roomsMap.Clear();

            _currentCenterRoomNum = 0;
        }


    }
}