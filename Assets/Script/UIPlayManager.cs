//using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Elven_Path
{
    public class UIPlayManager : MonoBehaviour
    {
        public static System.Action<bool> EnableBattlePanelEvent;

        public static System.Action<string> UpdatePlayerStatusTextEvent;
        public static System.Action<string> UpdateScoreTextEvent;
        public static System.Action<string> UpdateWaveTextEvent;
        public static System.Action<bool, string> UpdateHighScoreTextEvent;

        public static System.Action TurnOnEndPanelEvent;

        public static System.Action ShakePlayerPanelEvent;
        public static System.Action ShakeEnemyPanelEvent;

        public static System.Action<Sprite, string, string> BeginUpdatePlayerBattlePanelEvent;
        public static System.Action<Sprite, string, string> BeginUpdateEnemyBattlePanelEvent;

        public static System.Action<string> UpdatePlayerBattleHeartEvent;
        public static System.Action<string> UpdateEnemyBattleHeartEvent;

        [System.Serializable]
        public class UIModel
        {
            public TextMeshProUGUI playerStatusTextMesh, enemyStatusTextMesh;
            public GameObject playerBattlePanel, enemyBattlePanel;
            public Image playerImage, enemyImage;
            public TextMeshProUGUI playerHeartText, enemyHeartText;
            public TextMeshProUGUI enemyStatusText, playerStatusText;
            public TextMeshProUGUI current_scoreText1, current_scoreText2, high_scoreText, waveText;
            public GameObject new_record_Title, endPanel;
        }

        public UIModel uModel;

        #region Singleton
        private void Awake()
        {
            
        }
        private void OnEnable()
        {
            EnableBattlePanelEvent += EnableBattlePanel;

            UpdatePlayerStatusTextEvent += SetPlayerStatus;

            BeginUpdatePlayerBattlePanelEvent += BeginUpdatePlayerBattlePanel;
            BeginUpdateEnemyBattlePanelEvent += BeginUpdateEnemyBattlePanel;

            UpdatePlayerBattleHeartEvent += UpdatePlayerBattleHeart;
            UpdateEnemyBattleHeartEvent += UpdateEnemyBattleHeart;

            ShakePlayerPanelEvent += ShakePlayerPanel;
            ShakeEnemyPanelEvent += ShakeEnemyPanel;

            UpdateWaveTextEvent += UpdateWaveText;
            UpdateHighScoreTextEvent += UpdateHighScore;
            UpdateScoreTextEvent += UpdateScore;

            TurnOnEndPanelEvent += TurnOnEndPanel;
        }
        private void OnDisable()
        {
            EnableBattlePanelEvent -= EnableBattlePanel;

            UpdatePlayerStatusTextEvent -= SetPlayerStatus;

            BeginUpdatePlayerBattlePanelEvent -= BeginUpdatePlayerBattlePanel;
            BeginUpdateEnemyBattlePanelEvent -= BeginUpdateEnemyBattlePanel;

            UpdatePlayerBattleHeartEvent -= UpdatePlayerBattleHeart;
            UpdateEnemyBattleHeartEvent -= UpdateEnemyBattleHeart;

            ShakePlayerPanelEvent -= ShakePlayerPanel;
            ShakeEnemyPanelEvent -= ShakeEnemyPanel;

            UpdateWaveTextEvent -= UpdateWaveText;
            UpdateHighScoreTextEvent -= UpdateHighScore;
            UpdateScoreTextEvent -= UpdateScore;

            TurnOnEndPanelEvent -= TurnOnEndPanel;
        }
        private void Start()
        {
            //uModel.high_scoreText.text 
            UpdateScore("0");
        }
        #endregion

        #region BattlePanel
        public void EnableBattlePanel(bool on)
        {
            uModel.playerBattlePanel.SetActive(on);
            uModel.enemyBattlePanel.SetActive(on);
        }
        private void BeginUpdatePlayerBattlePanel(Sprite icon, string heart, string status)
        {
            uModel.playerImage.sprite = icon;
            uModel.playerHeartText.text = "x" + heart;
            uModel.playerStatusText.text = status;
        }
        private void BeginUpdateEnemyBattlePanel(Sprite icon, string heart, string status)
        {
            uModel.enemyImage.sprite = icon;
            uModel.enemyHeartText.text = "x"+heart;
            uModel.enemyStatusText.text = status;
        }
        private void UpdatePlayerBattleHeart(string text) { uModel.playerHeartText.text = "x"+text; }
        private void UpdateEnemyBattleHeart(string text) { uModel.enemyHeartText.text = "x"+text; }
        private void ShakePlayerPanel() { StartCoroutine(ShakePanel(uModel.playerBattlePanel.transform, 0.4f, 2)); }
        private void ShakeEnemyPanel() { StartCoroutine(ShakePanel(uModel.enemyBattlePanel.transform, 0.4f, 2)); }
        private IEnumerator ShakePanel (Transform obj, float duration, float magnitude)
        {
            Vector3 originalPos = obj.localPosition;
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                //float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                obj.localPosition = new Vector3(originalPos.x, y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            obj.localPosition = originalPos;
        }
        #endregion
        #region System
        private void SetPlayerStatus(string any_status) { uModel.playerStatusTextMesh.text = any_status; }
        public void Restart() { SceneManager.LoadScene(1); }
        public void Quit() { Application.Quit(); }
        #endregion
        #region ScorePanel
        private void TurnOnEndPanel() { uModel.endPanel.SetActive(true); }
        private void UpdateScore(string scoreText)
        {
            uModel.current_scoreText1.text = scoreText;
            uModel.current_scoreText2.text = scoreText;
        }
        //private bool newScore = false;
        private void UpdateHighScore(bool nScore, string highScoreText)
        {
            uModel.new_record_Title.SetActive(nScore);
            uModel.high_scoreText.text = highScoreText;
        }
        private void UpdateWaveText(string waveText) { uModel.waveText.text = waveText; }
        #endregion
    }
}