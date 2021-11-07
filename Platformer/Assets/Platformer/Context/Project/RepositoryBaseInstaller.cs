using UnityEngine;
using Zenject;

public class RepositoryBaseInstaller : MonoInstaller
{
    [SerializeField] private RepositoryBase repositoryBase;
    public override void InstallBindings()
    {
        Container.Bind<RepositoryBase>().FromComponentInNewPrefab(repositoryBase).AsSingle();
    }
}