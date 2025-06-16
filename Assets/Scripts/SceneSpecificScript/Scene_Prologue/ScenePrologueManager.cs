using System;
using System.Collections;
using System.Collections.Generic;
using AssetManagement;
using Entity;
using EventSystem.Dialogue;
using EventSystem.UI;
using Febucci.UI;
using GeneralManagers;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace SceneSpecificScript.Scene_Prologue
{
    public class ScenePrologueManager : MonoBehaviour
    {
        [Header("Dialogue")] [SerializeField] private TextAsset inkDialogue;
        [SerializeField] private String knotName = "IntroScene";

        [Header("Music")] [SerializeField] private AudioClip introMusic;

        [Header("Floating Text")] [SerializeField]
        private string floatingTextString = "Mirror, mirror, on the wall...";

        [SerializeField] private string floatingTextString2 = "Who's the fairest of them all?";

        [Header("Fade In")] [SerializeField] private float fadeOutDuration = 2f;
        [SerializeField] private float fadeInDuration = 4f;


        private async void Start()
        {
            await AddressablesManager.Initialized;
            await AddressablesManager.Instance.DownloadDependenciesAsync(
                new[] {HelperLabel.Scene_Prologue });
            UIManager.Instance.DisableOnScreenUI();
            GameManager.Instance.InputManager.DisableOpenMenuInput();
            GameManager.Instance.AudioManager.PlayMusic(introMusic);
            GameManager.Instance.PlayerManager.DisablePlayer();
            CGFadeEventSystem.InvokeFade(
                fadeOutDuration,
                fadeInDuration,
                null,
                InvokeEnterIntroSceneDialogue,
                new List<string> { floatingTextString, floatingTextString2 },
                new List<float> { 0.05f, 0.05f },
                false
            );
        }


        private void InvokeEnterIntroSceneDialogue()
        {
            DialogueEventSystem.InvokeEnterDialogue(
                new DialogueEventSystem.EnterDialogueEventArgs(inkDialogue, knotName));
        }


        private void OnDestroy()
        {
            UIManager.Instance.EnableOnScreenUI();
            GameManager.Instance.InputManager.EnableOpenMenuInput();
            GameManager.Instance.PlayerManager.EnablePlayer();
            AddressablesManager.Instance.ReleaseDependencies(
                new[] { HelperLabel.Scene_Prologue }
            );
        }
    }
}