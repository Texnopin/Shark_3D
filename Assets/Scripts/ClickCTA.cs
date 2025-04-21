using UnityEngine;

public class ClickCta : MonoBehaviour
{
    [SerializeField] private RectTransform playNowButton;
    [SerializeField] private RectTransform mainFoneImage;

    private float posOffset;
    private bool isLandscape = false;

    private void Start()
    {
        posOffset = mainFoneImage.localPosition.y;
    }

    private void Update()
    {
        isLandscape = IsLandscape();

        float h = Screen.height;
        float w = Screen.width;
        if (isLandscape)
        {
            /*mainFoneImage.localScale = new Vector3(3.5f + (-3.5f * (h / w)), 3.5f + (-3.5f * (h / w)), 3.5f + (-3.5f * (h / w)));
            mainFoneImage.localPosition = new Vector3(mainFoneImage.localPosition.x, posOffset + 30f, mainFoneImage.localPosition.z);*/

            playNowButton.localPosition = new Vector3(401, -190, 0);
        }
        else
        {
            /*mainFoneImage.localScale = new Vector3(0.50f + (0.48f * (h / w)), 0.50f + (0.48f * (h / w)), 0.50f + (0.48f * (h / w)));
            mainFoneImage.localPosition = new Vector3(mainFoneImage.localPosition.x, posOffset, mainFoneImage.localPosition.z);*/

            playNowButton.localPosition = new Vector3(0, -476, 0);

        }
    }

    bool IsLandscape()
    {
        return Screen.width > Screen.height;
    }

    public void Click()
    {
        Luna.Unity.Playable.InstallFullGame();
    }
}