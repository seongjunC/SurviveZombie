using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StoryView : MonoBehaviour
    {
        private readonly string[,] KR_Story = new string[15,2]
        {
            {"제임스","'좀비 사태가 발발한지 어언 반년... 세상은 혼란 속에 빠져들었다.'"},
            {"제임스","'이 나라에서 시작된 좀비들은 전세계로 퍼져나갔다.\n하지만 좀비들이 치명적이라고 해도 무기를 통한 반격을 진행하기 시작했고 속히 진정될 듯 보였다.'"},
            {"제임스","'하지만 그때 인류를 절망으로 몰아넣은 것이 변종 좀비들이다.\n변종 좀비들은 강력한 전투력과 특수한 능력들을 가지고 있었다.'"},
            {"제임스","'좀비들이 지능이 있는지는 알 수 없지만 좀비들은 사람들이 많은 곳을 주로 습격을 진행했고 \n남은 인류는 뿔뿔히 흩어지게 되었다.'"},
            {"제임스","'날이 갈수록 부족해지는 물자는 남은 사람들끼리 마저도 반목하게 만들었다.'"},
            {"???","... 아무리 너라지만 그 곳은 너무 위험해. 빨리 일을 마치고 안전하게 돌아오라고"},
            {"제임스","'방금 말한 사람은 나의 동료 도리스다.\n많은 사람들이 소속된 무리는 찾기 힘들지만 그래도 소규모로 뭉쳐다니는 사람들은 많다.'"},
            {"제임스","어쩔 수 없잖아. 우리도 이대로 가다간 같이 죽고 말거야.\n좀비 사태는 이 근방에서 시작됐고 딱 봐도 수상해 보이잖아."},
            {"제임스","이 절망적인 상황에서 조금이라도 희망을 찾아봐야 하지 않겠어?"},
            {"제임스","'이 곳은 좀비 사태가 발발한 곳으로 추정되는 지역 근방에 존재하는 한 연구소다.\n나는 혹시 모를 치료제를 찾아 이곳에 들어왔다.'"},
            {"도리스","후 너를 누가 말리겠냐. 그래서 나도 가는 건 말리지 않았잖아.\n어쨌건 몸 성히 돌아오기나 하라고"},
            {"제임스","'이 연구소 안에 치료제가 있을지는 모르겠지만 한번 탐색해볼까'"},
            {"튜토리얼", "캐릭터는 WASD를 통해서 조작할 수 있습니다.\n스페이스 바를 눌러 구르기를 하여 적의 공격을 피할 수 있습니다."},
            {"튜토리얼", "마우스 우측 키를 누르는 것으로 조준 모드에 진입할 수 있습니다.\n조준 모드에서는 카메라가 확대되며 해당 상태에서 좌클릭 시 총을 발사합니다."},
            {"튜토리얼", "R키를 누르는 것으로 재장전을 할 수 있습니다.\n잔탄의 경우는 하단 바 UI의 중앙에서 확인할 수 있고, 만약 잔탄이 0인 상태라면 이 또한 재장전에 진입합니다."},
        };
        private readonly string[,] EN_Story = new string[15,2]
        {
            {"James","'It has been almost half a year since the zombie outbreak... The world has fallen into chaos.'"},
            {"James","'The zombies that began in this country spread throughout the world. Even though the zombies were deadly, a counterattack using weapons began, and it seemed like the situation would quickly be brought under control.'"},
            {"James","'But what drove humanity into despair were the mutant zombies. The mutant zombies possessed powerful combat abilities and special skills.'"},
            {"James","'It's unknown whether the zombies have intelligence, but they mainly targeted areas with many people, causing the remaining human population to scatter far and wide.'"},
            {"James","'The materials that grew scarcer by the day caused even the survivors to turn against each other.'"},
            {"???","... Even for you, that place is too dangerous. Finish what you're doing quickly and come back safe."},
            {"James","'The person who just spoke is my colleague, Doris. Large organized groups are hard to find, but there are still many people moving around in small clusters.'"},
            {"James","I don't have a choice, do I? If we keep going like this, we'll all die together. \nThe zombie outbreak started near here, and it looks suspicious, doesn't it?"},
            {"James","Shouldn't we try to find even a sliver of hope in this desperate situation?"},
            {"James","'This place is a laboratory located near the area where the zombie outbreak is presumed to have started. I came here hoping to find a cure, just in case.'"},
            {"Doris","Sigh. Who could ever stop you? That's why I didn't forbid you from going. Just make sure you return in one piece."},
            {"James","'I don't know if there will be a cure inside this lab, but should I start exploring?'"},
            {"Tutorial", "The character can be controlled using WASD. Press the Space Bar to Dodge Roll and avoid enemy attacks."},
            {"Tutorial", "You can enter Aim Mode by pressing the Right Mouse Button. The camera will zoom in while in Aim Mode, and pressing the Left Mouse Button in this state will fire your weapon."},
            {"Tutorial", "Press the R Key to Reload your weapon. Your current ammo count can be checked in the center of the bottom bar UI. If your ammo count is zero, you will automatically enter the reload sequence."},
        };

        [SerializeField] private GameObject canvas;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button SkipButton;

        public void Awake()
        {
            SkipButton.onClick.AddListener(StoryOff);
            Time.timeScale = 0f;
        }
        public void SetStory(int line)
        {
            bool isEng = GlobalStateManager.Instance.voiceType == VoiceType.English;
            if (isEng)
            {
                nameText.text = EN_Story[line,0];
                text.text = EN_Story[line,1];
                SoundManager.Instance.PlayVoice(line+1, VoiceType.English);
            }
            else
            {
                nameText.text = KR_Story[line,0];
                text.text = KR_Story[line,1];
                SoundManager.Instance.PlayVoice(line+1, VoiceType.Korean);
            }
        }

        public void StoryOff()
        {
            Time.timeScale = 1f;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            SoundManager.Instance.StopVoice();
            
            canvas.gameObject.SetActive(false);
        }
    }
}