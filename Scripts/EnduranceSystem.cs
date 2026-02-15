using TMPro;
using UnityEngine;
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
            if(endurance + myInterface.changingEndurance <= 100)
                endurance += myInterface.changingEndurance;
            TMProTextManager.ChangeText(endurance, enduranceText);
        }
    }
}
