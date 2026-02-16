using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static AudioManager;
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
            //if(hp + myInterface.changingHP<=100)
            //    hp+= myInterface.changingHP;
            switch (hp + myInterface.changingHP)
            {
                case <= 0:
                    Debug.Log("добавить что-нибудь про смерть");
                    break;
                case <=100:
                    hp += myInterface.changingHP;
                    break;
                case > 100:
                    hp = 100;
                    break;
            }
            switch (myInterface.changingHP)
            {
                case < 0:
                    Instance.Play(SoundType.Explosion, this.transform.position);
                    break; 
                case >0:
                    Instance.Play(SoundType.UI, this.transform.position);
                    break;
                default:
                    break;
            }

            TMProTextManager.ChangeText(hp, hpText);
        }
    }
}