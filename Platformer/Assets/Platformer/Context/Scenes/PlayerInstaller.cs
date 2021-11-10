using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerController playerController;
    public override void InstallBindings()
    {
        Container.BindInstance(playerController).AsSingle().NonLazy();
    }
}