using System;
using UnityEngine;

namespace Samples.EventSystem
{
    public sealed class ManagedEventListener : MonoBehaviour
    {
        [SerializeField] private FirstManagedClickDetector _managedClickDetector;

        private void OnEnable()
        {
            FcbUtils.EventSystem.Notifier.Instance.AddListener<FirstEventArgs>(OnFirstEventArgs);
            FcbUtils.EventSystem.Notifier.Instance.AddListener<SecondEventArgs>(OnSecondEventArgs);
            FcbUtils.EventSystem.Notifier.Instance.AddListener<FirstEventArgs>(OnFirstOrSecondEventArgs);
            FcbUtils.EventSystem.Notifier.Instance.AddListener<SecondEventArgs>(OnFirstOrSecondEventArgs);

            FcbUtils.EventSystem.Notifier.Instance.AddListener<FirstEventArgs>(_managedClickDetector.gameObject.GetInstanceID(), OnSpecificObjectFirstEventArgs);
        }

        private void OnDisable()
        {
            FcbUtils.EventSystem.Notifier.Instance.RemoveListener<FirstEventArgs>(OnFirstEventArgs);
            FcbUtils.EventSystem.Notifier.Instance.RemoveListener<SecondEventArgs>(OnSecondEventArgs);
            FcbUtils.EventSystem.Notifier.Instance.RemoveListener<FirstEventArgs>(OnFirstOrSecondEventArgs);
            FcbUtils.EventSystem.Notifier.Instance.RemoveListener<SecondEventArgs>(OnFirstOrSecondEventArgs);

            if (_managedClickDetector != null)
            {
                FcbUtils.EventSystem.Notifier.Instance.RemoveListener<FirstEventArgs>(_managedClickDetector.gameObject.GetInstanceID(), OnSpecificObjectFirstEventArgs);
            }

            // Optionally
            FcbUtils.EventSystem.Notifier.Instance.Reset();
        }

        private void OnFirstEventArgs(object sender, EventArgs e)
        {
            Debug.Log($"Managed static event FirstEventArgs from: {sender}");
        }

        private void OnSecondEventArgs(object sender, EventArgs e)
        {
            Debug.Log($"Managed static event SecondEventArgs from {sender}");
        }

        private void OnFirstOrSecondEventArgs(object sender, EventArgs e)
        {
            Debug.Log($"Managed static event FirstOrSecondEventArgs: {sender}, type: {e.GetType()}");
        }

        private void OnFirstOrSecondEventArgs(object sender, FirstEventArgs e)
        {
            Debug.Log($"Managed static event FirstOrSecondEventArgs: {sender}, type: {e.GetType()}");
        }

        private void OnSpecificObjectFirstEventArgs(object sender, FirstEventArgs e)
        {
            Debug.Log($"OnSpecificObjectFirstEventArgs: {sender}, type: {e.GetType()}");
        }
    }
}