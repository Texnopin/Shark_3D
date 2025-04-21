using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class PartTwo : GamePartBase
{
    private const string Walk = "Walk";
    //private const string AsSharkRaftdamaged = "AS_Shark_RaftDamaged";
    //private const string AsSharkAttaced = "AS_Shark_RaftBiteCycle";
    private bool _isTap;
    [SerializeField] private Animator _tutorial;
    [SerializeField] private Animator sharkAnimator;
    [SerializeField] private Animator spearAnimator;
    [SerializeField] private GameObject hand;

    [SerializeField] private GameObject layerWater_1;
    [SerializeField] private GameObject layerWater_2;
    [SerializeField] private GameObject layerWater_3;

    private void Awake()
    {
        hand.SetActive(false);
        spearAnimator.gameObject.SetActive(true);
    }

    private void Start()
    {
        layerWater_1.SetActive(false);
        layerWater_2.SetActive(false);
        layerWater_3.SetActive(false);
        sharkAnimator.SetBool("BiteRaft", true);
    }

    public override void Init(GamePartConfig config)
    {
        base.Init(config);
        if (CameraAnimator != null && CameraAnimator.gameObject != null && CameraAnimator.gameObject.activeInHierarchy)
        {
            CameraAnimator.Play(Walk);
        }
        else
        {
            Debug.LogWarning("Cannot play animation on inactive animator: CameraAnimator");
        }

        //SharkAnimator.Play(AsSharkAttaced);
    }

    public void Init(Animator cameraAnimator, Animator dogAnimator, Animator hookAnimator, SoundManager soundManager,
        GameObject dog, Button button, GameObject textDefend, GameObject textSuccess, Animator sharkAnimator)
    {
        var config = GamePartConfig.Create(
            cameraAnimator,
            dogAnimator,
            hookAnimator,
            soundManager,
            dog,
            button,
            textDefend,
            textSuccess,
            sharkAnimator
        );

        Init(config);
        if (CameraAnimator != null && CameraAnimator.gameObject != null && CameraAnimator.gameObject.activeInHierarchy)
        {
            CameraAnimator.Play(Walk);
        }
    }

    protected override void OnActionButtonClick()
    {
        if (gameObject != null && !_isTap)
        {
            _tutorial.SetBool("TapAnim", false);
            _tutorial.gameObject.SetActive(false);
            _isTap = true;
            //CameraAnimator.Play(Hook);
            spearAnimator.SetBool("Attack", true);
            SoundManager.PlayHookSound();
            SoundManager.PlayTakeSound();
            StartCoroutine(PlayAnimationSequence());
        }
    }


    protected override IEnumerator PlayAnimationSequence()
    {
        yield return new WaitForSeconds(0.4f);

        if (SharkAnimator != null && SharkAnimator.gameObject != null && SharkAnimator.gameObject.activeInHierarchy)
        {
            //SharkAnimator.Play(AsSharkRaftdamaged);
            sharkAnimator.SetBool("BiteRaft", false);
        }

        if (TextToDefend != null)
        {
            TextToDefend.SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        if (TextSuccess != null)
        {
            TextSuccess.SetActive(true);
        }

        yield return new WaitForSeconds(1.2f);

        if (TextSuccess != null)
        {
            TextSuccess.SetActive(false);
        }

        CompleteCurrentPart();
    }

    protected override IEnumerator PlayTextAnimationsSequentially()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DelayedButtonClick(4));

        if (TextToDefend != null)
        {
            ActionButton.interactable = true;
            TextToDefend.SetActive(true);
        }
    }

    private IEnumerator DelayedButtonClick(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!_isTap) Tutorial();//OnActionButtonClick(); ������ ��� ����� ��������
    }

    protected override string GetActionButtonText()
    {
        return "Tap to defend";
    }
    private void Tutorial()
    {
        _tutorial.gameObject.SetActive(true);
        _tutorial.SetBool("TapAnim", true);
    }
}