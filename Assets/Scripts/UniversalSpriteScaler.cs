using UnityEngine;
using UnityEngine.UI;

public class UniversalSpriteScaler : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Vector2 _padding = Vector2.zero; // Отступы в единицах Canvas

    private RectTransform _rectTransform;
    private Image _image;
    private CanvasScaler _canvasScaler;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();

        if (_canvas == null)
            _canvas = GetComponentInParent<Canvas>();

        if (_canvas != null)
            _canvasScaler = _canvas.GetComponent<CanvasScaler>();

        ScaleToFillScreen();
    }


    private void Update()
    {
        if(Screen.width > Screen.height)
        {
            _padding.x = 58f;
        }
        else
        {
            _padding.x = 149.9f;
        }

        ScaleToFillScreen();
    }

    public void ScaleToFillScreen()
    {
        if (_canvas == null || _image == null || _image.sprite == null)
            return;

        Vector2 spriteSize = new Vector2(_image.sprite.rect.width, _image.sprite.rect.height);

        Vector2 targetSize;

        if (_canvasScaler != null && _canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
        {
            // Используем Reference Resolution из Canvas Scaler
            targetSize = _canvasScaler.referenceResolution - _padding * 2;
        }
        else
        {
            // Используем размер RectTransform канваса
            RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
            targetSize = canvasRect.rect.size - _padding * 2;
        }

        float scaleX = targetSize.x / spriteSize.x;
        float scaleY = targetSize.y / spriteSize.y;

        // Берём максимальный масштаб, чтобы покрыть весь экран
        float scale = Mathf.Max(scaleX, scaleY);

        _rectTransform.sizeDelta = spriteSize * scale;

        // Отключаем Preserve Aspect, чтобы не мешало
        if (_image.preserveAspect)
            _image.preserveAspect = false;
    }
}
