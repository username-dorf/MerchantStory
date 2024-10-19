using Core.Input;
using MVVM;
using UnityEngine;
using Zenject;

namespace UI
{
    public sealed class BindersInstaller : Installer<BindersInstaller>
    {
        public override void InstallBindings()
        {
            BinderFactory.RegisterBinder<TextBinder>();
            BinderFactory.RegisterBinder<ImageBinder>();
            BinderFactory.RegisterBinder<ButtonBinder>();
            BinderFactory.RegisterBinder<ViewSetterBinder<bool>>();
            BinderFactory.RegisterBinder<ViewSetterBinder<string>>();
            BinderFactory.RegisterBinder<ViewSetterBinder<Vector3>>();
            BinderFactory.RegisterBinder<ViewSetterBinder<SwipeProgress>>();
            //BinderFactory.RegisterBinder<ReactiveCollectionBinder<ProductView, ProductViewModel>>();
        }
    }
}