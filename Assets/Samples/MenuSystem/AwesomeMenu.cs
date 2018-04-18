using FcbUtils.MenuSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.MenuSystem
{
    public class AwesomeMenu : Menu<AwesomeMenu>
    {
        public Image Background;
        public Text Title;

        public static void Show(float awesomeness)
        {
            Open();

            Instance.Background.color = new Color32((byte)(129 * awesomeness), (byte)(197 * awesomeness), (byte)(34 * awesomeness), 255);
            Instance.Title.text = string.Format($"This menu is {awesomeness} awesome");
        }

        public static void Hide()
        {
            Close();
        }
    }
}