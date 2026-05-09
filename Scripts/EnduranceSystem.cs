using TMPro;
using UnityEngine;
using static AudioManager;
public interface IEnduranceInterface
{
    int changingEndurance { get; }
}
public class EnduranceSystem : MonoBehaviour
{
    [SerializeField] private int endurance = 100;
    [SerializeField] private TMP_Text enduranceText;
    public static EnduranceSystem enduranceSystemScript;
    private void OnEnable()
    {
        TouchSystemGameObject.OnOstrichBeakTouch += ReactionOnTouch;
    }
    private void OnDisable()
    {
        TouchSystemGameObject.OnOstrichBeakTouch -= ReactionOnTouch;
    }
    private void Start()
    {
        enduranceSystemScript = this;
        TMProTextManager.ChangeText(endurance, enduranceText);
    }
    private void ReactionOnTouch(GameObject obj)
    {
        if (obj.TryGetComponent<IEnduranceInterface>(out var myInterface))
        {
            //if(endurance + myInterface.changingEndurance <= 100)
            //    endurance += myInterface.changingEndurance;

            switch (endurance + myInterface.changingEndurance)
            {
                case <= 0:
                    endurance = 0;
                    break;
                case <= 100:
                    endurance += myInterface.changingEndurance;
                    break;
                case > 100:
                    endurance = 100;
                    break;
            }
            switch (myInterface.changingEndurance)
            {
                case < 0:
                    Instance.Play(SoundType.Explosion, this.transform.position);
                    break;
                case > 0:
                    Instance.Play(SoundType.UI, this.transform.position);
                    break;
                default:
                    break;
            }
            TMProTextManager.ChangeText(endurance, enduranceText);
        }
    }

    public static bool JumpRequest()
    {
        if (enduranceSystemScript.endurance >= 25)
        {
            enduranceSystemScript.endurance = enduranceSystemScript.endurance - 25;
            TMProTextManager.ChangeText(enduranceSystemScript.endurance, enduranceSystemScript.enduranceText);
            return true;
        }
        else
            return false;
    }
}
