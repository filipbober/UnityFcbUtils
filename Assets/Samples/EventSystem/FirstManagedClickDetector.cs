using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Samples.EventSystem
{
    public class FirstManagedClickDetector : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            FcbUtils.EventSystem.Notifier.Instance.Raise(this, EventArgs.Empty);
            FcbUtils.EventSystem.Notifier.Instance.Raise(this, new FirstEventArgs());

            FcbUtils.EventSystem.Notifier.Instance.Raise(this, gameObject.GetInstanceID(), new FirstEventArgs());
        }
    }
}