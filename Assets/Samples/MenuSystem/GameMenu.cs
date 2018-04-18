using FcbUtils.MenuSystem;

namespace Samples.MenuSystem
{
    public class GameMenu : SimpleMenu<GameMenu>
    {
        public override void OnBackPressed()
        {
            PauseMenu.Show();
        }
    }
}
