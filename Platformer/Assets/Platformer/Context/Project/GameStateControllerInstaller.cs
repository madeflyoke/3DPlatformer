using UnityEngine;
using Zenject;

public class GameStateControllerInstaller : MonoInstaller
{
    [SerializeField] private GameStateController gameStateController;
    public override void InstallBindings()
    {
        Container.Bind<GameStateController>().FromComponentInNewPrefab(gameStateController).AsSingle().NonLazy();
    }
}