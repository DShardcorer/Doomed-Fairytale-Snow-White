using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using EventSystem.Dialogue;
using Febucci.UI;
using GeneralManagers;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.SceneSpecificScript.IntroScene
{
    public class IntroSceneManager : MonoBehaviour
    {
        [Header("Dialogue")] [SerializeField] private TextAsset inkDialogue;
        [SerializeField] private String knotName = "RandomMan";

        [Header("Music")] [SerializeField] private AudioClip introMusic;

        [Header("Floating Text")] [SerializeField]
        private TextAnimator_TMP floatingText;

        [SerializeField] private TypewriterByCharacter typewriter;
        [SerializeField] private string floatingTextString = "Mirror, mirror, on the wall...";
        [SerializeField] private string floatingTextString2 = "Who's the fairest of them all?";

        [Header("Fade In")] [SerializeField] private Image fadeInPanel;
        private float fadeInDuration = 4f;

        private Coroutine fadeInCoroutine;

        private void Awake()
        {
            typewriter.onTextShowed.AddListener(OnTextShowed);
        }

        private void Start()
        {
            UIManager.Instance.DisableOnScreenUI();
            GameManager.Instance.InputManager.DisableOpenMenuInput();
            GameManager.Instance.AudioManager.PlayMusic(introMusic);
            GameManager.Instance.PlayerManager.DisablePlayer();
            typewriter.ShowText(floatingTextString);
        }

        private void OnTextShowed()
        {
            if (String.Equals(floatingText.TMProComponent.text, floatingTextString))
            {
                typewriter.ShowText(floatingTextString2);
            }
            else
            {
                //Start the fade in effect
                typewriter.StartDisappearingText();
                fadeInCoroutine = StartCoroutine(FadeIn());
            }
        }


        private void OnDestroy()
        {
            UIManager.Instance.EnableOnScreenUI();
            GameManager.Instance.InputManager.EnableOpenMenuInput();
            GameManager.Instance.PlayerManager.EnablePlayer();
        }

        private IEnumerator FadeIn()
        {
            while (fadeInPanel.color.a >= 0)
            {
                Color color = fadeInPanel.color;
                color.a -= Time.deltaTime / fadeInDuration;
                fadeInPanel.color = color;
                yield return null;
            }

            DialogueEventSystem.InvokeEnterDialogue(
                new DialogueEventSystem.EnterDialogueEventArgs(inkDialogue, knotName));
            Destroy(fadeInPanel);
        }
    }
}