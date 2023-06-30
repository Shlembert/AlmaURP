using UnityEngine;

public class TextureChange : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private Texture texture;

    public void Press()
    {
        material.SetTexture("_BgTexture", texture);

        Debug.Log("Set texture: " + texture.name);
    }
}