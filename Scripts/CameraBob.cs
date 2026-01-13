using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [Header("Bob settings")]
    [SerializeField] private float walkBobSpeed = 10f;   // Скорость покачивания
    [SerializeField] private float walkBobAmount = 0.05f; // Амплитуда (высота)

    [Header("Side sway")]
    [SerializeField] private float swayAmount = 0.03f;   // Боковое покачивание

    [Header("Smoothing")]
    [SerializeField] private float smooth = 8f;           // Плавность возврата

    private Vector3 startLocalPos; // Начальная локальная позиция
    private float timer;

    [SerializeField] private float velocity=0.1f;
    [SerializeField] private float previousBobY=1f;
    [SerializeField] private bool lowestBobY=false;

    private void Awake()
    {
        startLocalPos = transform.localPosition;
    }
    private void Start()
    {
        ScoreCounter.OnTick += UpdateBob2;
    }
    private void OnDestroy()
    {
        ScoreCounter.OnTick -= UpdateBob2;
    }

    public void UpdateBob2(byte _byte)
    { 
        velocity = _byte / 10f;
    }
    public void Update()
    {
        timer += Time.deltaTime * walkBobSpeed * velocity;

        float bobY = Mathf.Sin(timer) * walkBobAmount;
        float swayX = Mathf.Cos(timer * 0.5f) * swayAmount; 
        
        if (IsAtLowestPoint(bobY))
        {
            Debug.Log("Камера в нижней точке шага!");
            AudioManager.Instance.Play(SoundType.Footstep, gameObject.transform.position);
        }

        Vector3 targetPos = startLocalPos + new Vector3(swayX, bobY, 0f);
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Time.deltaTime * smooth
        );
    }
    bool IsAtLowestPoint(float currentBobY)
    {
        bool isLowest=false;

        //если отметка "внизу" уже сработала
        if (lowestBobY == true)
        {
            //ищем прошлую точку выше нинешней
            isLowest = previousBobY > currentBobY &&    // было ниже
                 Mathf.Abs(previousBobY) > 0.001f; // защита от шумов
        }
        else //если отметка "внизу" не сработала
        {
            //ищем нинешнюю точку выше прошлой
            isLowest = previousBobY < currentBobY &&    // было ниже
                 Mathf.Abs(previousBobY) > 0.001f; // защита от шумов
        }

        //если нашли нужную точку
        if (isLowest==true)
        {
            //если отметка "внизу" уже сработала
            if (lowestBobY == true)
            {
                //выключаем отметку "внизу"
                lowestBobY = false;
            }
            else //если отметка "внизу" не сработала
            {
                //включаем отметку "внизу" и отправляем true
                lowestBobY = true;
                return true;
            }

        }
        previousBobY = currentBobY; 
        return false;
    }
}