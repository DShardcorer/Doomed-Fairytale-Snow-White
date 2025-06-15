using System.Collections;
using Helpers;
using TMPro;
using UnityEngine;

namespace General
{
    public class DamagePopupUI : MonoBehaviour
    {
        private DamagePopupUIManager _damagePopupUIManager;
        public TextMeshProUGUI damageText;
        public CanvasGroup canvasGroup;
    
        [SerializeField] private float floatDuration = 1f;
        [SerializeField] private float floatDistance = 1f;

        public void Initialize(DamagePopupUIManager parent, float damage)
        {
            _damagePopupUIManager = parent;
            //Round the damage to the nearest 1 precision float
            damage = Mathf.Round(damage * 10f) / 10f;

            canvasGroup.alpha = 1f;
            if (damage > 0)
            {
                damageText.text = damage.ToString();
                damageText.color = Color.red;
            }
            else
            {
                damageText.text = (-damage).ToString();
                damageText.color = Color.green;
            }
            StopAllCoroutines();
            StartCoroutine(FloatUpAndFade());
        }

        private IEnumerator FloatUpAndFade()
        {
            float elapsed = 0f;
            Vector3 offset = new Vector3(0, 0.5f, 0);
            Vector3 startPos = transform.position + offset;
            Vector3 endPos = startPos + new Vector3(0, floatDistance, 0);

            while (elapsed < floatDuration)
            {
                elapsed += Time.deltaTime;
                float time = elapsed / floatDuration;
                transform.position = Vector3.Lerp(startPos, endPos, time);
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, time);
                yield return null;
            }
            _damagePopupUIManager.PoolManager.ReturnObject(HelperUIName.DamagePopupUI, gameObject);
        
        }
    }
}
