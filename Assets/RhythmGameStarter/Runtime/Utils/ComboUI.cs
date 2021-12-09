using UnityEngine;


namespace RhythmGameStarter
{

    public class ComboUI : MonoBehaviour
    {
        public string[] animatorParams;
        public GameObject UIHEART;
        private Animator anim;

        void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void OnComboAdd()
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }
            anim.SetTrigger(animatorParams[0]);
        }

        public void OnVisibilityChanged(bool show)
        {
            anim.SetBool(animatorParams[1], show);
        }

        public void Update() {
        }
    }
}