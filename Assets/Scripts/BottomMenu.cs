using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomMenu : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private Slider slider;
    [SerializeField] private Image imgLight, imgBtn;
    private bool _isUp = false;
    private List<ClickHandler> clickHandlers = new List<ClickHandler>();

    public void StartGame()
    {
        foreach (var item in gridGenerator.Pazzles)
            clickHandlers.Add(item.GetComponent<ClickHandler>());
       
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        Color currentColor = imgLight.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, value);
        imgLight.DOColor(targetColor, 0.01f);
    }

    public void MoveMenu()
    {
        _isUp = !_isUp;

        float pos;
        float flip;

        if (!_isUp)
        {
            pos = -9.2f;
            flip = 1;
        }
        else
        {
            pos = -4.5f;
            flip = -1;
        }

        transform.DOMoveY(pos, 0.5f, false).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            imgBtn.transform.localScale = new Vector3(1, flip, 1);
            DragPazzlesSelector(!_isUp);
        });
    }

    private void DragPazzlesSelector(bool value)
    {
        foreach (var item in clickHandlers) item.IsReady = value;
    }
}
