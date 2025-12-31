using System;
using Manager;
using Player;
using UI;
using UnityEngine;

namespace Scene
{
    public class StoryController : MonoBehaviour
    {
        private int lineCount = 0;
        [SerializeField] private StoryView storyView;
        private PlayerController player;
        

        public void OnEnable()
        {
            player = GlobalStateManager.Instance.player;
            player.OnPlayerInit += storyStart;
        }

        private void storyStart()
        {
            player.OnPlayerInit -= storyStart;
            
            storyView.SetStory(lineCount++);
            
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

        }

        public void NextLine()
        {
            if (lineCount == 15)
            {
                DisableStoryUi();
                return;
            }
            
            storyView.SetStory(lineCount++);
        }

        public void DisableStoryUi()
        {
            storyView.StoryOff();
        }
    }
}