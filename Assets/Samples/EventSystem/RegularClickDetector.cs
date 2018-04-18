using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Samples.EventSystem
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class RegularClickDetector : MonoBehaviour, IPointerClickHandler
    {
        public event EventHandler Clicked;
        public static event EventHandler ClickedStatic;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked(EventArgs.Empty);
            OnStaticClicked(EventArgs.Empty);
        }

        private void OnClicked(EventArgs e)
        {
            Clicked?.Invoke(this, e);
        }

        private void OnStaticClicked(EventArgs e)
        {
            ClickedStatic?.Invoke(this, e);
        }
    }
}