using UnityEngine;

public class ChangingENDURANCEObjScript : MonoBehaviour, IEnduranceInterface
{
    public int changingEndurance { get { return changingEndur; } }

    [SerializeField] private int changingEndur;
}
