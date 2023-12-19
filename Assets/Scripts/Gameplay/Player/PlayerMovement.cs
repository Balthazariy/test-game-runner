using TestGame.Settings;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _minZ = 1.8f;
    [SerializeField] private float _maxZ = -1.8f;
    [SerializeField] private float _speed = 5.0f;

    [Space(4)]
    [Header("Android swipe settings")]
    [SerializeField] private float swipeThreshold = 50f;

    private Vector2 touchStartPos;

    private bool _isActive;

    private void OnEnable()
    {
        EventBus.OnGameStateChangedEvent += OnGameStateChangedEventHandler;
    }

    private void OnDisable()
    {
        EventBus.OnGameStateChangedEvent -= OnGameStateChangedEventHandler;
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * _speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.left * Time.deltaTime * _speed * -1);
        }

#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            var touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            var direction = touchPosition - transform.localPosition;
            //var clampedDirection = Mathf.Clamp(direction.x, -1.0f, 1.0f);
            transform.Translate(Vector3.left * Time.deltaTime * _speed * (-direction.x * 4));
        }
#endif
        if (transform.localPosition.z > _minZ)
        {
            transform.localPosition = new Vector3(0, 0, _minZ);
        }

        if (transform.localPosition.z < _maxZ)
        {
            transform.localPosition = new Vector3(0, 0, _maxZ);
        }
    }

    private void OnGameStateChangedEventHandler(GameStates gameStates)
    {
        _isActive = gameStates == GameStates.Run;
    }
}