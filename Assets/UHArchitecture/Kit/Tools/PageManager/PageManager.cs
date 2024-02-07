using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UralHedgehog
{
    [Serializable]
    public class PageManager
    {
        [SerializeField] private List<Page> _pages;
        
        public int CurrentPageIndex { get; private set; }

        public void Init(int defaultEnablePage = 0)
        {
            for (var i = 0; i < _pages.Count; i++)
            {
                _pages[i].Init(i);
                if (_pages[i].Tab != null)
                {
                    _pages[i].Tab.onEnable += EnablePage;
                }
            }

            EnablePage(defaultEnablePage, true);
        }

        public void Hide()
        {
            foreach (var page in _pages)
            {
                page.Hide(false, true);
            }
        }

        public void Unsubscribe()
        {
            foreach (var page in _pages.Where(page => page.Tab != null))
            {
                page.Tab.onEnable -= EnablePage;
            }
        }

        protected internal void EnablePage(int pageId)
        {
            for (var i = 0; i < _pages.Count; i++)
            {
                if (i == pageId)
                {
                    CurrentPageIndex = i;
                    _pages[i].Show();
                }
                else _pages[i].Hide();
            }
        }

        private void EnablePage(int pageId, bool isInit)
        {
            for (var i = 0; i < _pages.Count; i++)
            {
                if (i == pageId)
                {
                    CurrentPageIndex = i;
                    _pages[i].Show(isInit);
                }
                else _pages[i].Hide(isInit);
            }
        }

        [Serializable]
        private class Page
        {
            [SerializeField] private string _name;
            [SerializeField] private GameObject _page;
            [SerializeField] private Tab _tab;
            public Tab Tab => _tab;

            public void Init(int index)
            {
                _tab.Init(index);
            }

            public void Show(bool isInit = false)
            {
                if (isInit)
                {
                    _page.GetComponent<Animator>().Play("ShowInit");
                    _tab.ChangeState(Tab.State.SELECTED);
                }
                else
                {
                    _page.GetComponent<Animator>().Play("Show");
                    _tab.ChangeState(Tab.State.SELECTED);
                }
            }

            public void Hide(bool isInit = false, bool isDisable = false)
            {
                if (isInit)
                {
                    _page.GetComponent<Animator>().Play("Normal");
                    _tab.ChangeState(Tab.State.AVAILABLE);
                }
                else
                {
                    _page.GetComponent<Animator>().Play("Hide");
                    if (!isDisable) _tab.ChangeState(Tab.State.AVAILABLE);
                }
            }
        }
    }
}