using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Elven_Path
{
    public class UIPlayManager : MonoBehaviour
    {
        public static Action<string, string> UpdatePlayerStatusTextEvent;
        public static Action<string, string> UpdateEnemyStatusTextEvent;

        public static Action ShakePlayerPanelEvent;
        public static Action ShakeEnemyPanelEvent;

        public static Action<Sprite, string> BeginUpdatePlayerBattleHeartEvent;
        public static Action<Sprite, string> BeginUpdateEnemyBattleHeartEvent;

        public static Action<string> UpdatePlayerBattleHeartEvent;
        public static Action<string> UpdateEnemyBattleHeartEvent;

        [System.Serializable]
        public class UIModel
        {
            public TextMeshProUGUI playerStatusTextMesh, enemyStatusTextMesh;
            public GameObject playerBattlePanel, enemyBattlePanel;
            public Image playerImage, enemyImage;
            public TextMeshProUGUI playerHeartText, enemyHeartText;
        }

        public UIModel uModel;

        #region Singleton
        private void Awake()
        {
            
        }
        private void OnEnable()
        {
            UpdatePlayerStatusTextEvent += SetPlayerStatus;
            UpdateEnemyStatusTextEvent += SetEnemyStatus;

            BeginUpdatePlayerBattleHeartEvent += BeginUpdatePlayerBattleHeart;
            BeginUpdateEnemyBattleHeartEvent += BeginUpdateEnemyBattleHeart;

            UpdatePlayerBattleHeartEvent += UpdatePlayerBattleHeart;
            UpdateEnemyBattleHeartEvent += UpdateEnemyBattleHeart;

            ShakePlayerPanelEvent += ShakePlayerPanel;
            ShakeEnemyPanelEvent += ShakeEnemyPanel;
        }
        private void OnDisable()
        {
            UpdatePlayerStatusTextEvent -= SetPlayerStatus;
            UpdateEnemyStatusTextEvent -= SetEnemyStatus;

            BeginUpdatePlayerBattleHeartEvent -= BeginUpdatePlayerBattleHeart;
            BeginUpdateEnemyBattleHeartEvent -= BeginUpdateEnemyBattleHeart;

            UpdatePlayerBattleHeartEvent -= UpdatePlayerBattleHeart;
            UpdateEnemyBattleHeartEvent -= UpdateEnemyBattleHeart;

            ShakePlayerPanelEvent -= ShakePlayerPanel;
            ShakeEnemyPanelEvent -= ShakeEnemyPanel;
        }
        private void Start()
        {
            
        }
        #endregion

        private void ShakePlayerPanel() { StartCoroutine(ShakeUI.Shake(uModel.playerBattlePanel.transform, 1, 2)); }
        private void ShakeEnemyPanel() { StartCoroutine(ShakeUI.Shake(uModel.enemyBattlePanel.transform, 1, 2)); }

        private void BeginUpdatePlayerBattleHeart(Sprite icon, string text)
        {
            uModel.playerImage.sprite = icon;
            uModel.playerHeartText.text = text;
        }
        private void BeginUpdateEnemyBattleHeart(Sprite icon, string text)
        {
            uModel.enemyImage.sprite = icon;
            uModel.enemyHeartText.text = text;
        }

        private void UpdatePlayerBattleHeart(string text)
        { uModel.playerHeartText.text = text; }
        private void UpdateEnemyBattleHeart(string text)
        { uModel.enemyHeartText.text = text; }

        private void SetPlayerStatus(string swordPower, string shieldPower)
        { uModel.playerStatusTextMesh.text = "<color=red>sword : " + swordPower + "</color>" + "<color=blue> shield : " + shieldPower + "</color>"; }

        private void SetEnemyStatus(string swordPower, string shieldPower)
        { uModel.enemyStatusTextMesh.text = "<color=red>" + swordPower + " : sword</color> " + "<color=blue>" + shieldPower + " : shield</color>"; }
    }
}