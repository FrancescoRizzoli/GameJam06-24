using Utility;

namespace UnityEngine.UI
{
    public class ButtonUIController : MonoBehaviour
    {
        [SerializeField]
        private ScreenType screenType;
        [SerializeField]
        private bool open = true;

        private void Awake()
        {
            Button button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnButtonClick);
            }
        }
        public void OnButtonClick()
        {
            UIManager.instance.ManageScreen(screenType,open);
        }

    }
}
