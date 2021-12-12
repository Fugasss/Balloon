using Core;
using FallingObject;
using Player;
using Score;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Game m_Game;
        [SerializeField] private Clicker m_Clicker;
        [SerializeField] private PlayerHealth m_PlayerHealth;
        [SerializeField] private BalloonDestroyEffectManager m_DestroyEffectManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Game>().FromInstance(m_Game).AsSingle();
            Container.Bind<Clicker>().FromInstance(m_Clicker).AsSingle();
            Container.Bind<PlayerHealth>().FromInstance(m_PlayerHealth).AsSingle();
            Container.Bind<BalloonDestroyEffectManager>().FromInstance(m_DestroyEffectManager).AsSingle();

            Container.BindInterfacesTo<ScoreChanger>().FromNew().AsSingle();
        }
    }
}