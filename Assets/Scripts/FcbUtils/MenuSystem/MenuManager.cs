using System;
using System.Collections.Generic;
using System.Linq;
using Samples.MenuSystem;
using UnityEngine;

namespace FcbUtils.MenuSystem
{
    public class MenuManager : MonoBehaviour
    {
        private string _menuResourcesPath;

        private readonly Dictionary<Type, Menu> _menusByType = new Dictionary<Type, Menu>();

        private readonly Stack<Menu> _menuStack = new Stack<Menu>();

        public static MenuManager Instance { get; private set; }

        protected void Awake()
        {
            // Set references here only
            Instance = this;
        }

        protected void Start()
        {
            // Initialize objects
            Init("Menus");
        }

        protected void OnDestroy()
        {
            Instance = null;
        }

        protected void Update()
        {
            // On Android the back button is sent as Esc
            if (Input.GetKeyDown(KeyCode.Escape) && _menuStack.Count > 0)
            {
                _menuStack.Peek().OnBackPressed();
            }
        }

        public void CreateInstance<T>() where T : Menu
        {
            var prefab = GetPrefab<T>();

            Instantiate(prefab, transform);
        }

        public void OpenMenu(Menu instance)
        {
            // Deactivate top menu
            if (_menuStack.Count > 0)
            {
                if (instance.DisableMenusUnderneath)
                {
                    foreach (var menu in _menuStack)
                    {
                        menu.gameObject.SetActive(false);

                        if (menu.DisableMenusUnderneath)
                            break;
                    }
                }

                var topCanvas = instance.GetComponent<Canvas>();
                var previousCanvas = _menuStack.Peek().GetComponent<Canvas>();
                topCanvas.sortingOrder = previousCanvas.sortingOrder + 1;
            }

            _menuStack.Push(instance);
        }

        public void CloseMenu(Menu menu)
        {
            if (_menuStack.Count == 0)
            {
                Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", menu.GetType());
                return;
            }

            if (_menuStack.Peek() != menu)
            {
                Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", menu.GetType());
                return;
            }

            CloseTopMenu();
        }

        public void CloseTopMenu()
        {
            var instance = _menuStack.Pop();

            if (instance.DestroyWhenClosed)
                Destroy(instance.gameObject);
            else
                instance.gameObject.SetActive(false);

            // Reactivate top menu
            // If a reactivated menu is an overlay we need to activate the menu under it
            foreach (var menu in _menuStack)
            {
                menu.gameObject.SetActive(true);

                if (menu.DisableMenusUnderneath)
                    break;
            }
        }

        protected void Init(string menuResourcesPath)
        {
            _menuResourcesPath = menuResourcesPath;

            LoadMenus();
            ShowTopMenu();
        }

        protected virtual void ShowTopMenu()
        {
            // TODO Find a way to get menu type in Init and show it here
            MainMenu.Show();
        }

        private void ShowTopMenuT<T>(SimpleMenu<T> menu) where T : SimpleMenu<T>
        {
            // todo
        }

        private void LoadMenus()
        {
            var menus = Resources.LoadAll<Menu>(_menuResourcesPath).ToList();
            Debug.Log($"Loaded {menus.Count} menus");

            foreach (var menu in menus)
            {
                var menuType = menu.GetType();
                if (!_menusByType.ContainsKey(menuType))
                {
                    _menusByType.Add(menuType, menu);
                }
                else
                {
                    Debug.LogWarning($"Menu type of {menuType} is already registered");
                }
            }
        }

        private T GetPrefab<T>() where T : Menu
        {
            var menuType = typeof(T);
            if (_menusByType.ContainsKey(menuType))
            {
                return _menusByType[menuType] as T;
            }

            throw new MissingReferenceException("Prefab not found for type " + menuType);
        }
    }
}