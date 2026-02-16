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
}
