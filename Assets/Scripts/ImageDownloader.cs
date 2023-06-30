using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageDownloader : MonoBehaviour
{
    [SerializeField] private string serverURL = "http://data.ikppbb.com/test-task-unity-data/pics/";
    [SerializeField] private GameObject rawImagePrefab;
    [SerializeField] private Transform content;
    [SerializeField] private int imagesCount;
    [SerializeField] private GameObject downloadScreen;
    [SerializeField] private Image fill;

    private int nextImageIndex = 1;

    private void Start()
    {
        CreateEmptyPrefabs();
        StartCoroutine(AnimateDownloadScreen());
    }

    private void CreateEmptyPrefabs()
    {
        for (int i = 0; i < imagesCount; i++)
        {
            GameObject rawImageObject = Instantiate(rawImagePrefab, content);
            rawImageObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void RequestNextImage(Action<Texture2D> callback)
    {
        if (nextImageIndex > imagesCount)
        {
            Debug.LogError("All images have been requested.");
            callback(null);
            return;
        }

        GameObject rawImageObject = content.GetChild(nextImageIndex - 1).gameObject;

        string imageURL = serverURL + nextImageIndex.ToString() + ".jpg";
        StartCoroutine(DownloadImage(imageURL, rawImageObject, callback));

        nextImageIndex++;
    }

    private IEnumerator DownloadImage(string imageURL, GameObject rawImageObject, Action<Texture2D> callback)
    {
        using (var uwr = UnityWebRequestTexture.GetTexture(imageURL))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download image: " + uwr.error);
                callback(null);
                yield break;
            }

            Texture2D texture = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
            callback(texture);
        }
    }

    private IEnumerator AnimateDownloadScreen()
    {
        float duration = 2f;
        float elapsedTime = 0f;
        float startFillAmount = fill.fillAmount;
        float targetFillAmount = 1f;

        while (elapsedTime < duration)
        {
            float fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);
            fill.fillAmount = fillAmount;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fill.gameObject.SetActive(false);

        // Fade out animation
        CanvasGroup canvasGroup = downloadScreen.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            // Deactivate the download screen
            downloadScreen.SetActive(false);
        });
    }
}
