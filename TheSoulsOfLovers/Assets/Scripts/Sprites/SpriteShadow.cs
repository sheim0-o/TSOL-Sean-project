using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    private SpriteRenderer sprRndCaster;
    private SpriteRenderer sprRndShadow;
    private Transform transCaster;
    private Transform transShadow;

    public Vector2 offset = new Vector2((float)0.15, (float)-0.2);
    public Vector3 rotation = new Vector3((float)60, (float)0, (float)-15);

    void Start()
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
        sprRndShadow.sortingOrder = sprRndCaster.sortingOrder-1;
        sprRndShadow.color = new Color(0f, 0f, 0f, 0.5f);
        sprRndShadow.drawMode = sprRndCaster.drawMode;
        sprRndShadow.size = sprRndCaster.size;
    }

    void LateUpdate()
    {
        transShadow.position = new Vector2(transCaster.position.x + offset.x, transCaster.position.y + offset.y);
        sprRndShadow.sprite = sprRndCaster.sprite;
    }
}
