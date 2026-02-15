using TMPro;
using UnityEngine;
public interface IHPInterface
{
    int changingHP { get; }
}

public class HPSystem:MonoBehaviour
{
    [SerializeField] private int hp = 100;
    [SerializeField] private TMP_Text hpText;
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
        TMProTextManager.ChangeText(hp, hpText);
    }
    private void ReactionOnTouch(GameObject obj)
    {
        if (obj.TryGetComponent<IHPInterface>(out var myInterface))
        {
            if(hp + myInterface.changingHP<=100)
                hp+= myInterface.changingHP;
            TMProTextManager.ChangeText(hp, hpText);
        }
    }
}