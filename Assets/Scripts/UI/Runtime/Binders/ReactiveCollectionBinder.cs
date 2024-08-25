using System;
using System.Collections.Generic;
using MVVM;
using UI.Views;
using UniRx;
using UnityEngine;

namespace UI
{
    public sealed class ReactiveCollectionBinder<TView, TModel> : IBinder where TView : Component
    {
        private readonly CollectionView<TView> collectionView;
        private readonly IReadOnlyReactiveCollection<TModel> collectionModel;

        private readonly Dictionary<TModel, (TView, IBinder)> elements = new();
        private readonly List<IDisposable> disposables = new();

        public ReactiveCollectionBinder(CollectionView<TView> collectionView, IReadOnlyReactiveCollection<TModel> collectionModel)
        {
            this.collectionView = collectionView;
            this.collectionModel = collectionModel;
        }

        void IBinder.Bind()
        {
            this.collectionModel.ObserveAdd().Subscribe(v => this.OnItemAdded(v.Value)).AddTo(this.disposables);
            this.collectionModel.ObserveRemove().Subscribe(v => this.OnItemRemoved(v.Value)).AddTo(this.disposables);
            
            foreach (TModel item in this.collectionModel)
            {
                TView view = this.collectionView.AddItem();
                IBinder binder = BinderFactory.CreateComposite(view, item);
                binder.Bind();
            
                this.elements.Add(item, (view, binder));
            }
        }

        void IBinder.Unbind()
        {
            foreach ((_, IBinder binder) in this.elements.Values)
            {
                binder.Unbind();
            }

            this.collectionView.Clear();
            this.elements.Clear();
            this.disposables.ForEach(it => it.Dispose());
        }

        private void OnItemAdded(TModel item)
        {
            if (this.elements.ContainsKey(item))
            {
                return;
            }

            TView view = this.collectionView.AddItem();
            BinderComposite binder = BinderFactory.CreateComposite(view, item);
            this.elements.Add(item, (view, binder));

            binder.Bind();
        }

        private void OnItemRemoved(TModel item)
        {
            if (this.elements.Remove(item, out (TView view, IBinder binder) tuple))
            {
                tuple.binder.Unbind();
                this.collectionView.RemoveItem(tuple.view);
            }
        }
    }
}

// this.Container
//     .BindFactory<Product, ProductViewModel, ProductViewModel.Factory>()
//     .AsSingle()
//     .NonLazy();
//
// this.Container
//     .BindInterfacesAndSelfTo<ProductsViewModel>()
//     .AsSingle()
//     .NonLazy();