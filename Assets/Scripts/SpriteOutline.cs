using System.Linq;
using UnityEngine;

//[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    [Header("Sprite Outline Settings")]
    public Color color = Color.white;
    [Range(0, 16)]
    public int outlineSize = 1;


    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }
    

    void OnEnable()
    {
        UpdateOutline(true);
    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    void Update()
    {
        UpdateOutline(true);
    }

    void UpdateOutline(bool outline)
    {
        var mpb = new MaterialPropertyBlock();

        _spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        _spriteRenderer.SetPropertyBlock(mpb);
    }
}
