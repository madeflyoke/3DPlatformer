using UnityEngine;
using Zenject;

public class UIControllerInstaller : MonoInstaller
{
    [SerializeField] private UIController uIController;
    public override void InstallBindings()
    {
        Container.Bind<UIController>().FromComponentInNewPrefab(uIController).AsSingle().NonLazy();
    }
}