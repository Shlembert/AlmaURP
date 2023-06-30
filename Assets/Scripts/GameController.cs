using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private GridPlacement left, right;
    [SerializeField] private RectTransform leftOffsetSceen, rightOffsetScreen;
    [SerializeField] private Transform leftAnchor, rightAnchor, winPanel;
    [SerializeField] private SpriteRenderer centralSprite;
    [SerializeField] private BottomMenu bottomMenu;
    [SerializeField] private Image screen;

    private int _count;
    private int _winCount;

    private List<Vector2> points = new List<Vector2>();

    public async void StartGame()
    {
        GridAdaptation();

        _count = gridGenerator.GetSizeGrid();
        _winCount = _count * _count * 2;

        int halfCount = _count * _count / 2 + 1;

        points = left.PlacePoints(halfCount);
        points.AddRange(right.PlacePoints(halfCount));

        await UniTask.Delay(1000);
        centralSprite.DOFade(0.2f, 1f);
        RandomMovePazzles();
        bottomMenu.StartGame();
        ClickHandler.OnDrag += HandleDrag;
        PazzleController.ItemPlacedEvent += HandleItemPlaced;
    }

    private void OnDestroy()
    {
        ClickHandler.OnDrag -= HandleDrag;
        PazzleController.ItemPlacedEvent -= HandleItemPlaced;
        DOTween.KillAll();
    }

    private void HandleDrag(bool isPressed)
    {
        float duration = 0.5f;
        if (isPressed) centralSprite.DOFade(0, duration);
        else centralSprite.DOFade(0.2f, duration);
    }

    private void RandomMovePazzles()
    {
        List<PazzleController> pazzles = gridGenerator.Pazzles;

        List<int> usedIndexes = new List<int>();

        foreach (PazzleController pazzle in pazzles)
        {
            int index;
            do
            {
                index = Random.Range(0, points.Count);
            }
            while (usedIndexes.Contains(index));

            usedIndexes.Add(index);

            Vector3 go = points[index];

            pazzle.RandomPos = go;
            pazzle.MoveToPoint(go, 1f);
            pazzle.ZoomDownHandler(0.4f);
        }
    }

    private void GridAdaptation()
    {
        leftAnchor.position = leftOffsetSceen.position;
        rightAnchor.position = rightOffsetScreen.position;
    }

    private void HandleItemPlaced()
    {
        if (_winCount > 2) _winCount--;
        else WinGame();
    }

    private async void WinGame()
    {
        await UniTask.Delay(500);
        DOTween.KillAll();
        _winCount = _count * _count * 2;

        winPanel.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

