using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class PartFour : GamePartBase
{
    private const string TakeState = "TakeBarrel";
    private const string SeeLoot = "SeeLoot";
    private int _isTap = 0;

    [SerializeField] private Animator barrelAnimator;
    [SerializeField] private Animator boxAnimator;
    [SerializeField] private Animator hudAnimator;
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject finishButton;
    [SerializeField] private RectTransform finishSplashImage;

    [SerializeField] private Animator _tutorial;
    [SerializeField] private Button tap;

    [SerializeField] private GameObject layerWater_1;
    [SerializeField] private GameObject layerWater_2;
    [SerializeField] private GameObject layerWater_3;

    private bool barellTaked = false;
    private bool hook = true;

    private void Start()
    {
        layerWater_1.SetActive(false);
        layerWater_2.SetActive(false);
        layerWater_3.SetActive(false);
        _tutorial.gameObject.GetComponent<RectTransform>().position = tap.gameObject.GetComponent<RectTransform>().position;
    }

    protected override void OnPartStart()
    {
        if (ActionButton != null && ActionButton.gameObject != null)
        {
            ActionButton.gameObject.SetActive(true);
        }

        if (hand != null)
        {
            hand.SetActive(true);
        }

        StartCoroutine(DelayedButtonClick(4));

        base.OnPartStart();
    }

    protected override string StateName => "WalkToTake";

    private IEnumerator DelayedButtonClick(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_isTap != 1) Tutorial();//OnActionButtonClick();
    }

    private IEnumerator DelayedButtonClick2(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_isTap != 2 && _isTap == 1) Tutorial();//OnActionButtonClick();
    }

    protected override void OnActionButtonClick()
    {
        if (gameObject != null && (_isTap != 1))
        {
            _isTap = 1;
            StartCoroutine(PlayAnimationSequence());
        }
        else if (gameObject != null && (_isTap == 1) && barellTaked)
        {
            _isTap = 2;
            StartCoroutine(PlayAnimationSequence2(hook));
        }
    }

    private void CompleteAfterDelayAlternative()
    {
        box.SetActive(true);

        if (TextSuccess != null)
        {
            TextSuccess.SetActive(true);
        }

        //CompleteCurrentPart();
    }

    private void CompleteAfterDelayAlternative2()
    {
        box.SetActive(false);

        if (TextSuccess != null)
        {
            TextSuccess.SetActive(true);
        }

        CompleteCurrentPart();
    }

    protected override IEnumerator PlayAnimationSequence()
    {
        _tutorial.SetBool("TapAnim", false);
        _tutorial.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);

        if (CameraAnimator != null && CameraAnimator.gameObject != null && CameraAnimator.gameObject.activeInHierarchy)
        {
            CameraAnimator.Play(Hook);
        }
        else
        {
            Debug.LogWarning("Cannot play animation on inactive animator: CameraAnimator");
        }

        yield return new WaitForSeconds(0.5f);

        if (barrelAnimator != null && barrelAnimator.gameObject != null && barrelAnimator.gameObject.activeInHierarchy)
        {
            barrelAnimator.Play(TakeState);
        }
        else
        {
            Debug.LogWarning("Cannot play animation on inactive animator: barrelAnimator");
        }

        if (gameObject != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(CompleteAfterDelay());
            barrelAnimator.SetBool("TakeBox", true);
            //barrelAnimator.Play("TakeBarrel");
            //barrel.SetActive(false);
        }
        else
        {
            //CompleteAfterDelayAlternative();
        }
    }

    private IEnumerator PlayAnimationSequence2(bool _hook)
    {
        _tutorial.SetBool("TapAnim", false);
        _tutorial.gameObject.SetActive(false);
        if (_hook == false) yield break;

        yield return new WaitForSeconds(0.2f);

        if (CameraAnimator != null && CameraAnimator.gameObject != null && CameraAnimator.gameObject.activeInHierarchy)
        {
            //CameraAnimator.SetBool("Hook2", _hook);
            //hook = false;
            OpenGame();
        }
        else
        {
            Debug.LogWarning("Cannot play animation on inactive animator: CameraAnimator");
        }

        yield return new WaitForSeconds(0.5f);


        if (gameObject != null && gameObject.activeInHierarchy)
        {
            //StartCoroutine(CompleteAfterDelay2());
            //OpenGame();
            //boxAnimator.SetBool("TakeBox", true);
            //boxAnimator.Play("takeBox");
            //box.SetActive(false);
        }
        else
        {
            CompleteAfterDelayAlternative2();
        }
    }

    protected override IEnumerator PlayTextAnimationsSequentially()
    {
        yield return new WaitForSeconds(0.5f);

        if (TextToDefend != null)
        {
            TextToDefend.SetActive(true);
            ActionButton.interactable = true;
        }
    }

    protected override string GetActionButtonText()
    {
        return "Tap to collect";
    }

    private IEnumerator CompleteAfterDelay()
    {
        _tutorial.gameObject.SetActive(false);
        if (TextToDefend != null)
        {
            TextToDefend.SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        if (TextSuccess != null)
        {
            //CameraAnimator.Play(SeeLoot);
            TextSuccess.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        if (TextSuccess != null)
        {
            TextSuccess.SetActive(false);
        }

        box.SetActive(true);
        TextToDefend.SetActive(true);
        StartCoroutine(DelayedButtonClick2(4));
        barellTaked = true;
        //OpenGame();
    }

    private IEnumerator CompleteAfterDelay2()
    {
        _tutorial.gameObject.SetActive(false);
        if (TextToDefend != null)
        {
            TextToDefend.SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        if (TextSuccess != null)
        {
            //CameraAnimator.Play(SeeLoot);
            TextSuccess.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        if (TextSuccess != null)
        {
            TextSuccess.SetActive(false);
        }

        OpenGame();
    }

    private void OpenGame()
    {
        Destroy(_tutorial.gameObject);
        Luna.Unity.LifeCycle.GameEnded();
        /*if(Screen.width > Screen.height)
        {
            finishSplashImage.localScale = new Vector3(2.3f, 2.3f, 2.3f);
        }
        else
        {
            finishSplashImage.localScale = new Vector3(1.6f, 1.6f, 1.6f);
        }*/
        finishSplashImage.gameObject.SetActive(true);
        finishButton.gameObject.SetActive(true);
        hudAnimator.Play("PlayNow");
    }

    private void Tutorial()
    {
        _tutorial.gameObject.SetActive(true);
        _tutorial.SetBool("TapAnim", true);
    }
}


