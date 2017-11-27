using Lix.NightGame.Game;
using UnityEngine;
using UnityEngine.UI;
namespace Lix.NightGame.UI
{
    public class GUIManager : MonoBehaviour
    {
        public Text hpWizard;
        public Text hpCalcifer;

        public void OnEnable()
        {
            GameManager.S_Instance.UpdateHpWizardDelegate += UpdateHpWizardText;
            GameManager.S_Instance.UpdateHpCalciferDelegate += UpdateHpCalciferText;
        }
        public void OnDisable()
        {
            GameManager.S_Instance.UpdateHpWizardDelegate -= UpdateHpWizardText;
            GameManager.S_Instance.UpdateHpCalciferDelegate -= UpdateHpCalciferText;
        }
        public void UpdateHpWizardText(int hp)
        {
            hpWizard.text = "" + hp;
        }
        public void UpdateHpCalciferText(int hp)
        {
            hpCalcifer.text = "" + hp;
        }

    }
}
