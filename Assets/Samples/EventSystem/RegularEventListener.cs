using System;
using UnityEngine;

namespace Samples.EventSystem
{
    public sealed class RegularEventListener : MonoBehaviour
    {
        [SerializeField] private RegularClickDetector _regularClickDetector;

        private void OnEnable()
        {
            RegularClickDetector.ClickedStatic += HandleStaticObjectClicked;
            _regularClickDetector.Clicked += HandleObjectClicked;
        }

        private void OnDisable()
        {
            RegularClickDetector.ClickedStatic -= HandleStaticObjectClicked;
            _regularClickDetector.Clicked -= HandleObjectClicked;
        }

        private void HandleStaticObjectClicked(object sender, EventArgs e)
        {
            Debug.Log($"Regular static event fired from {sender}");
        }

        private void HandleObjectClicked(object sender, EventArgs e)
        {
            Debug.Log($"Regular event fired from {sender}");
        }
    }
}