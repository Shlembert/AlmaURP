using DG.Tweening;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void LoadScene( int index)
    {
        DOTween.KillAll();
        SceneManagerController.Instance.SwitchScene(index);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }
}
