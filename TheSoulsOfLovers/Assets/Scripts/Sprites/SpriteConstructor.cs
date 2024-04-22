using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteConstructor : MonoBehaviour
{
    [Header("Shadow")]
    public bool hasShadow = true;
    public bool takeChildrens = false;
    public Vector2 offset = new Vector2(0F, 0.15F);
    public Vector3 rotation = new Vector3(60F, 0F, -15F);

    private SpriteRenderer sprRndCaster;
    private SpriteRenderer sprRndShadow;
    private Transform transCaster;
    private Transform transShadow;

    [Space(10)]

    [Header("Render")]
    private int sortingOrderBase = 0;
    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        if (hasShadow)
        {
            transCaster = transform;
            transShadow = new GameObject().transform;
            transShadow.parent = transCaster;
            transShadow.gameObject.name = "Shadow";
            transShadow.Rotate(rotation.x, rotation.y, rotation.z);
            transShadow.localScale = new Vector3(1, 1, 1);

            sprRndCaster = GetComponent<SpriteRenderer>();
            sprRndShadow = transShadow.gameObject.AddComponent<SpriteRenderer>();

            //sprRndShadow.sortingLayerID = SortingLayer.NameToID("Layer 1");
            sprRndShadow.sortingLayerName = sprRndCaster.sortingLayerName;
            sprRndShadow.sortingOrder = sprRndCaster.sortingOrder - 1;
            sprRndShadow.color = new Color(0f, 0f, 0f, 0.5f);
            sprRndShadow.drawMode = sprRndCaster.drawMode;
            sprRndShadow.size = sprRndCaster.size;
        }
    }

    void LateUpdate()
    {
        if (hasShadow)
        {
            transShadow.position = new Vector2(transCaster.position.x + offset.x, transCaster.position.y + offset.y);
            sprRndShadow.sprite = sprRndCaster.sprite;
            sprRndShadow.color = new Color(0f, 0f, 0f, 0.5f);
        }
        else
        {
            if(sprRndShadow!=null)
                sprRndShadow.color = new Color(0f, 0f, 0f, 0f);
        }
    }
}
