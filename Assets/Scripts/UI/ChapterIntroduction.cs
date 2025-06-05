using System;
using System.Collections;
using Ink.Runtime;
using InputSystem;
using Other;
using TMPro;
using UI;
using UI.Interfaces;
using UnityEngine;

public class ChapterIntroduction : UserInterface
{ 
    public override UserInterfaceState State => UserInterfaceState.ChapterIntroduction;
    
    [SerializeField] private CanvasGroup textCanvasGroup;
    
    [SerializeField] private float textTransitionSpeed;
    [SerializeField] private float delayBetweenLines;

    [Header("DEV")] [SerializeField] private bool skip;

    private TextMeshProUGUI _introductionText;

    private Story _currentStory;
    
    public static event Action<IntroductionInfo> OnIntroductionStart;

    public static void InvokeOnIntroductionStart(IntroductionInfo introduction)
    {
        OnIntroductionStart?.Invoke(introduction);
    }
    
    public static event Action OnIntroductionFinish;

    public static void InvokeOnIntroductionFinish()
    {
        OnIntroductionFinish?.Invoke();
    }
    
    protected override void Awake()
    {
        base.Awake();
        _introductionText = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnIntroductionStart += StartIntroduction;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        OnIntroductionStart -= StartIntroduction;
    }
    
    public override void OnEnter()
    {
        CanvasGroup.alpha = 1;
        InputManager.Instance.DisableInputs();
        InputManager.Instance.Controlls.UI.Disable();
    }
    public override void OnExit()
    {
        CanvasGroup.alpha = 0;
        InputManager.Instance.EnableInputs();
        InputManager.Instance.Controlls.UI.Enable();
    }

    private void StartIntroduction(IntroductionInfo introduction)
    {
        if (skip)
        {
            EndIntroduction();
            return;
        }
        _currentStory = new Story(introduction.introductionText.text);

        StartCoroutine(ContinueIntroduction());
    }

    private IEnumerator ContinueIntroduction()
    {
        while (_currentStory.canContinue)
        {
            var nextLine = _currentStory.Continue();
            if (nextLine.Equals("") && !_currentStory.canContinue)
            {
                yield return StartCoroutine(FadeOutUI());
            }
            else
            {
                _introductionText.text = nextLine;
                yield return StartCoroutine(FadeInText());
                yield return new WaitForSeconds(delayBetweenLines);
                yield return StartCoroutine(FadeOutText());
                yield return new WaitForSeconds(0.5f);
            }
        }
        yield return StartCoroutine(FadeOutUI());
        EndIntroduction();
    }

    private void EndIntroduction()
    {
        InvokeOnIntroductionFinish();
        UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);
    }
    
    private IEnumerator FadeInText()
    {
        while (textCanvasGroup.alpha < 1)
        {
            textCanvasGroup.alpha += textTransitionSpeed * Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator FadeOutText()
    {
        while (textCanvasGroup.alpha > 0)
        {
            textCanvasGroup.alpha -= textTransitionSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOutUI()
    {
        while (CanvasGroup.alpha > 0)
        {
            CanvasGroup.alpha -= textTransitionSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
