using EventSystem.Player;
using GeneralManagers;
using UnityEngine;
using UnityEngine.UI;

namespace EntitySystems.VitalStatSystems.Mana_System
{
    public class ManaUI : MonoBehaviour, ILifecycle<UIManager>
    {
        private UIManager _parent;
        public UIManager Parent => _parent;
        [SerializeField] private int maxManaForReference = 500;

        [SerializeField] private GameObject manaBar;

        [SerializeField] private Image manaFill;

        public void Initialize(UIManager parent)
        {
            _parent = parent;
            manaFill.fillAmount = 1;
            PlayerVitalStatsEventSystem.OnManaChanged += ManaSystem_OnManaChanged;
        }

        private void ManaSystem_OnManaChanged(object sender, ManaChangedEventArgs e)
        {
            //Increase mana bar size according to max mana and max mana for reference
            manaBar.transform.localScale = new Vector3((float)e.MaxMana / maxManaForReference, 1, 1);

            manaFill.fillAmount = (float)e.CurrentMana / e.MaxMana;
        }

        public void Dispose()
        {
            _parent = null;
        }
    }
}
