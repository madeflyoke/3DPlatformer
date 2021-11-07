using UnityEngine;
using Zenject;

public class ScenesControllerInstaller : MonoInstaller
{
    [SerializeField] private ScenesController scenesController;
    public override void InstallBindings()
    {
        Container.Bind<ScenesController>().FromComponentInNewPrefab(scenesController).AsSingle().NonLazy();
    }
}