using System;
using MVVM;
using NaughtyAttributes;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace UI
{
    public sealed class MonoViewBinder : MonoBehaviour
    {
        private enum BindingMode
        {
            FromInstance = 0,
            FromResolve = 1,
            FromResolveId = 2
        }

        [SerializeField]
        private BindingMode viewBinding;

        [ShowIf(nameof(viewBinding), BindingMode.FromInstance)]
        [SerializeField]
        private Object view;

        private bool _viewBindingFromResolve => this.viewBinding == BindingMode.FromResolve;
        private bool _viewBindingFromResolveId => this.viewBinding == BindingMode.FromResolveId;
        [ShowIf(EConditionOperator.Or, "_viewBindingFromResolve", "_viewBindingFromResolveId")]
        [SerializeField]
        private TypeReference viewType;

        [ShowIf(nameof(viewBinding), BindingMode.FromResolveId)]
        [SerializeField]
        private string viewId;

        [Space(8)]
        [SerializeField]
        private BindingMode viewModelBinding;

        [ShowIf(nameof(viewModelBinding), BindingMode.FromInstance)]
        [SerializeField]
        private Object viewModel;

        private bool _viewModelBindingFromResolve => this.viewModelBinding == BindingMode.FromResolve;
        private bool _viewModelBindingFromResolveId => this.viewModelBinding == BindingMode.FromResolveId;
        [ShowIf(EConditionOperator.Or, "_viewModelBindingFromResolve", "_viewModelBindingFromResolveId")]
        [SerializeField]
        private TypeReference viewModelType;

        [ShowIf(nameof(viewModelBinding), BindingMode.FromResolveId)]
        [SerializeField]
        private string viewModelId;

        [Inject]
        private DiContainer diContainer;

        private IBinder _binder;

        private void Awake()
        {
            _binder = this.CreateBinder();
        }

        private void OnEnable()
        {
            _binder.Bind();
        }

        private void OnDisable()
        {
            _binder.Unbind();
        }

        private IBinder CreateBinder()
        {
            object view = this.viewBinding switch
            {
                BindingMode.FromInstance => this.view,
                BindingMode.FromResolve => this.diContainer.Resolve(this.viewType.Type),
                BindingMode.FromResolveId => this.diContainer.ResolveId(this.viewType.Type, this.viewId),
                _ => throw new Exception($"Binding type of view {this.viewBinding} is not found!")
            };

            object model = this.viewModelBinding switch
            {
                BindingMode.FromInstance => this.viewModel,
                BindingMode.FromResolve => this.diContainer.Resolve(this.viewModelType.Type),
                BindingMode.FromResolveId => this.diContainer.ResolveId(this.viewModelType.Type, this.viewModelId),
                _ => throw new Exception($"Binding type of view {this.viewBinding} is not found!")
            };

            return BinderFactory.CreateComposite(view, model);
        }
    }
}
