using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private int itemCount = 4;
    [SerializeField] private Transform pazzle, greedLine, difficultyScreen;
    [SerializeField] private float spriteQuadSize = 194;
    [SerializeField] private float ppu = 100;
    [SerializeField] private float scaleMultiplay = 1;
    [SerializeField] private float imageSize;
    [SerializeField] private GameController gameController;

    private float _offset;

    private List<PazzleController> _pazzles = new List<PazzleController>();
    public List<PazzleController> Pazzles { get => _pazzles; set => _pazzles = value; }

    public void SetCountButton(int count)
    {
        itemCount = count;
        PazzlePlace();

        difficultyScreen.DOMoveY(15, 0.5f, false).SetEase(Ease.InBack).OnComplete(()=> 
        {
            gameController.StartGame();
        });
    }

    private void PazzlePlace()
    {
        _offset = (spriteQuadSize / ppu) * imageSize / itemCount;

        float cur = -_offset * itemCount * 0.5f + _offset * 0.5f;

        Vector3 localOffset = new Vector3(cur, cur, 0);

        for (int x = 0; x < itemCount; x++)
        {
            for (int y = 0; y < itemCount; y++)
            {
                if (x == 0 && y == 0) SpawnItem(x, y, 0);
                else if (x == itemCount - 1 && y == itemCount - 1) SpawnItem(x, y, 8);
                else if (x == itemCount - 1 && y == 0) SpawnItem(x, y, 2);
                else if (x == 0 && y == itemCount - 1) SpawnItem(x, y, 6);
                else if (x == 0) SpawnItem(x, y, 3);
                else if (y == 0) SpawnItem(x, y, 1);
                else if (x == itemCount - 1) SpawnItem(x, y, 5);
                else if (y == itemCount - 1) SpawnItem(x, y, 7);
                else SpawnItem(x, y, 4);
            }
        }
       
        void SpawnItem(int x, int y, int index)
        {
            Vector3 pos = new Vector3(x, y, 0) * _offset + localOffset;

            SpriteRenderer SpawnSprite(Transform body)
            {
                SpriteRenderer renderer = Instantiate(body, pos, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
                renderer.sprite = sprites[index];
                renderer.transform.localScale = Vector3.one * _offset * 0.5f * scaleMultiplay;
                return renderer;
            }

            SpriteRenderer puzzleRender = SpawnSprite(pazzle);
            SpriteRenderer greedRenderer = SpawnSprite(greedLine);

            Pazzles.Add(puzzleRender.GetComponent<PazzleController>());

            Vector2 uvOffset = new Vector2(x / (float)itemCount + 0.5f / itemCount, y / (float)itemCount + 0.5f / itemCount);

            var propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetTexture("_MainTex", sprites[index].texture);
            propertyBlock.SetFloat("_PuzzleSize", itemCount);
            propertyBlock.SetVector("_UVOffset", uvOffset);
            propertyBlock.SetFloat("_UVScale", spriteQuadSize / ppu / scaleMultiplay);

            string textureData = PlayerPrefs.GetString("SavedTexture");
            byte[] textureBytes = System.Convert.FromBase64String(textureData);
            Texture2D savedTexture = new Texture2D(1, 1);
            savedTexture.LoadImage(textureBytes);
            propertyBlock.SetTexture("_Picture", savedTexture);

            puzzleRender.SetPropertyBlock(propertyBlock);
            greedRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    public int GetSizeGrid()
    {
        return itemCount;
    }
}