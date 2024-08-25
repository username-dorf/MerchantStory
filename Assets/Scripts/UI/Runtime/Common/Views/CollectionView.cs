using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Views
{
    [Serializable]
    public sealed class CollectionView<T> : IEnumerable<T> where T : Component
    {
        public int Count => this.items.Count;
        
        [SerializeField]
        private T itemPrefab;

        [SerializeField]
        private Transform container;

        private readonly List<T> items = new();
        
        public T AddItem()
        {
            T item = GameObject.Instantiate(this.itemPrefab, this.container);
            this.items.Add(item);
            return item;
        }

        public void RemoveItem(T item)
        {
            if (this.items.Remove(item))
            {
                GameObject.Destroy(item.gameObject);
            }
        }

        public void Clear()
        {
            foreach (var item in this.items)
            {
                GameObject.Destroy(item.gameObject);
            }

            this.items.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}