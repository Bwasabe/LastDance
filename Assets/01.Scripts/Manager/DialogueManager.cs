using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private Animator dialogueAnimator;
    [SerializeField]
    private DialogueDataListSO dialogueDataList;
    [SerializeField]
    private TextMeshProUGUI[] contents;
    [SerializeField]
    private GameObject next;

    private readonly int startHash = Animator.StringToHash("Start");
    private readonly int EndHash = Animator.StringToHash("End");
    private readonly int ExitHash = Animator.StringToHash("Exit");

    private CanvasGroup dialoguePanel;
    private RectTransform dialogueTransform;

    private int currentLV = 0;
    private bool startWrite = false;
    private bool writing = false;

    private List<Data> currentData;
    private int curIdx;
    private int line = -1;

    Sequence startSequence;
    Sequence endSequence;
    
    private void Start()
    {
        dialoguePanel = dialogueAnimator.GetComponent<CanvasGroup>();
        dialogueTransform = dialogueAnimator.GetComponent<RectTransform>();

        next.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            LoadDialogue();
        }

        if (Input.GetMouseButtonDown(0) && startWrite)
        {
            WriteContent();
        }
    }

    private void LoadDialogue()
    {
        InitDialogue();

        dialoguePanel.DOFade(1, 0.5f);

        currentData = dialogueDataList.DialogueDatas[currentLV].data;

        curIdx = 0;
        line = -1;
        writing = false;
        next.SetActive(false);

        startSequence = DOTween.Sequence();

        startSequence.Append(dialogueTransform.DOSizeDelta(new Vector2(200f, 200f), 0.15f));
        startSequence.AppendInterval(0.05f);
        startSequence.Append(dialogueTransform.DOSizeDelta(new Vector2(1200f, 200f), .35f).SetEase(Ease.InBack));
        startSequence.Append(dialogueTransform.DOSizeDelta(new Vector2(1200f, 250f), 0.15f).SetEase(Ease.OutBack));
        startSequence.AppendCallback(() =>
        {
            startWrite = true;
            WriteContent();
        });

        //dialogueAnimator.SetTrigger(startHash);

        //StartCoroutine("StartContent");
    }

    private void InitDialogue()
    {
        dialogueTransform.sizeDelta = new Vector2(200, 200);


        InitCotents();
    }

    private void InitCotents()
    {
        foreach(TextMeshProUGUI content in contents)
        {
            content.text = "";
        }
    }

    private void WriteContent()
    {
        if(writing)
        {
            contents[line].DOKill();
            contents[line].text = currentData[curIdx - 1].Content;
            writing = false;
            next.SetActive(true);
            return;
        }

        if(curIdx >= currentData.Count)
        {
            startWrite = false;
            InitCotents();
            next.SetActive(false);

            endSequence = DOTween.Sequence();

            endSequence.Append(dialogueTransform.DOSizeDelta(new Vector2(1200f, 200f), 0.15f).SetEase(Ease.OutBack));
            endSequence.Append(dialogueTransform.DOSizeDelta(new Vector2(200f, 200f), 0.25f).SetEase(Ease.InBack));
            endSequence.Append(dialoguePanel.DOFade(0, 0.2f));

            return;
        }

        writing = true;
        next.SetActive(false);

        switch (currentData[curIdx].type)
        {
            case ContentsType.Next:
                line++;
                break;
            case ContentsType.Clear:
                InitCotents();
                line = 0;
                break;
        }

        contents[line].DOText(currentData[curIdx++].Content, .5f).SetEase(Ease.Linear)
            .OnComplete(() => 
            {
                writing = false;
                next.SetActive(true);
            });
    }
}
