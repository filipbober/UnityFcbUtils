using System;
using UnityEngine;

namespace FcbUtils.Pooling.Sample
{
    public class Deactivator : MonoBehaviour
    {
        private async void OnEnable()
        {
            await TimeSpan.FromSeconds(10);

            // Prevent errors after game is stopped in editor
            if (this != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
