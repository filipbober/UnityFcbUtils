using FcbUtils.LevelManagement;
using UnityEngine;

namespace Samples.LevelManagement
{
    public class SceneSwitcher : MonoBehaviour
    {
        public void OnSwitchSceneClicked()
        {
            MainController.SwitchScene(Scenes.SecondScene);
        }
    }
}
