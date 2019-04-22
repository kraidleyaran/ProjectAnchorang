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
        //var mpb = new MaterialPropertyBlock();

        //_spriteRenderer.GetPropertyBlock(mpb);
        var material = _spriteRenderer.materials[2];
        material.SetFloat("_Outline", outline ? 1f : 0);
        material.SetColor("_OutlineColor", color);
        material.SetFloat("_OutlineSize", outlineSize);
        _spriteRenderer.material = material;
        //_spriteRenderer.SetPropertyBlock(mpb);
    }
}
