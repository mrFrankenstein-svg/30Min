using UnityEngine;

public class ChangingHPObjScript : MonoBehaviour,IHPInterface
{
    public int changingHP { get { return changingHp; } }
    [SerializeField] private int changingHp;
}
