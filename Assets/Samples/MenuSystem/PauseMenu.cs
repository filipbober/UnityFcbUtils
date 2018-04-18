using FcbUtils.MenuSystem;

namespace Samples.MenuSystem
{
    public class PauseMenu : SimpleMenu<PauseMenu>
    {
        public void OnQuitPressed()
        {
            Hide();
            Destroy(gameObject); // This menu does not automatically destroy itself

            GameMenu.Hide();
        }
    }
}