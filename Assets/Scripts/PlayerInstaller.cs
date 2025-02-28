using System.ComponentModel;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    public Transform HandPosition; 

    public override void InstallBindings()
    {
        Container.BindInstance(HandPosition).AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerMovement>().AsSingle();
        Container.BindInterfacesAndSelfTo<ItemInteraction>().AsSingle();
        Container.BindInterfacesAndSelfTo<DropButtonController>().AsSingle();
    }
}