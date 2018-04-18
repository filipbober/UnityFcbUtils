using FcbUtils.MenuSystem;
using UnityEngine;

namespace Samples.MenuSystem
{
    /// <summary>
    /// Code that does not depend on instance should be here
    /// </summary>
    public class MainMenu : SimpleMenu<MainMenu>
    {
        public void OnPlayPressed()
        {
            GameMenu.Show();
        }

        public void OnOptionsPressed()
        {
            OptionsMenu.Show();
        }

        public override void OnBackPressed()
        {
            Application.Quit();
        }
    }
}