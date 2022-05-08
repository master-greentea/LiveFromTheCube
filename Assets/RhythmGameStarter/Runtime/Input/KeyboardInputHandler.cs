using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    [HelpURL("https://bennykok.gitbook.io/rhythm-game-starter/hierarchy-overview/input")]
    public class KeyboardInputHandler : BaseInputHandler
    {
        [Title("Track Action Key", 4)]
        [ReorderableDisplay("Track")]
        public StringList keyMapping;

        private KeyCode[] _bosuKeys = new KeyCode[3];

        [Title("Swipe Action Key", 4)]
        public KeyCode up;
        public KeyCode down;
        public KeyCode left;
        public KeyCode right;

        [System.Serializable]
        public class StringList : ReorderableList<string> { }

        public override bool GetTrackActionKeyDown(Track track, int index)
        {
            //var key = keyMapping[index];
            //return Input.GetKeyDown(key);
            return Input.GetKeyDown(_bosuKeys[index]);
        }

        public override bool GetTrackActionKeyUp(Track track, int index)
        {
            //var key = keyMapping[index];
            //return Input.GetKeyUp(key);
            return Input.GetKeyUp(_bosuKeys[index]);
        }

        public override bool GetTrackActionKey(Track track, int index)
        {
            //var key = keyMapping[index];
            //return Input.GetKey(key);
            return Input.GetKey(_bosuKeys[index]);
        }

        public override bool GetTrackDirectionKey(Note.SwipeDirection swipeDirection)
        {
            KeyCode key = KeyCode.None;
            switch (swipeDirection)
            {
                case Note.SwipeDirection.Up:
                    key = up;
                    break;
                case Note.SwipeDirection.Down:
                    key = down;
                    break;
                case Note.SwipeDirection.Left:
                    key = left;
                    break;
                case Note.SwipeDirection.Right:
                    key = right;
                    break;
            }
            return Input.GetKey(key);
        }


        public void UpdateBosuKeys(KeyCode[] newKeys)
        {
            _bosuKeys = newKeys;
        }

    }
}