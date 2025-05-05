using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using EventSystem.Dialogue;
using EventSystem.UI;
using Febucci.UI;
using GeneralManagers;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.SceneSpecificScript.IntroScene
{
    public class IntroSceneManager : MonoBehaviour
    {
        [Header("Dialogue")] [SerializeField] private TextAsset inkDialogue;
        [SerializeField] private String knotName = "IntroScene";

        [Header("Music")] [SerializeField] private AudioClip introMusic;

        [Header("Floating Text")]
        [SerializeField] private string floatingTextString = "Mirror, mirror, on the wall...";
        [SerializeField] private string floatingTextString2 = "Who's the fairest of them all?";

        [Header("Fade In")]
        [SerializeField] private float fadeOutDuration = 2f;
        [SerializeField] private float fadeInDuration = 4f;
        

        private void Start()
        {
            UIManager.Instance.DisableOnScreenUI();
            GameManager.Instance.InputManager.DisableOpenMenuInput();
            GameManager.Instance.AudioManager.PlayMusic(introMusic);
            GameManager.Instance.PlayerManager.DisablePlayer();
            FadeEventSystem.InvokeFade(
                fadeOutDuration,
                fadeInDuration,
                null,
                InvokeEnterIntroSceneDialogue,
                new List<string> { floatingTextString, floatingTextString2 },
                new List<float> { 0.05f, 0.05f },
                true
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
        }
        
    }
}