using Core.Logic;
using Core.UI;
using Data;
using Infrastructure.States;
using Services.Economy;
using StaticData;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Meta.Hud
{
    public class HUDController : MonoBehaviour
    {
        private const string LoseText = "You've Lost and shoul start stage again.";
        private const string WinText = "You've and got some bucks.";

        [SerializeField] private ActorUI _heroUI;
        [SerializeField] private Button _returnButton;
        [SerializeField] private WindowBase _stagePopup;
        private IEconomyService _economyService;

        private GameStateMachine _stateMachine;

        [Inject]
        private void Construct(GameStateMachine stateMachine, IEconomyService economyService)
        {
            _stateMachine = stateMachine;
            _economyService = economyService;
        }

        public void Initialize(StageStaticData stageStaticData, StageProgressData stageProgressData)
        {
            SetupHeroUI(stageProgressData.Hero);
        }


        private void SetupHeroUI(GameObject hero)
        {
            _heroUI.Initialize(hero.GetComponent<IHealth>(), false);
        }
    }
}