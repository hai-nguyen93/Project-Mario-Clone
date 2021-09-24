using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    private TextMeshPro tmp;
    public float duration = 1f;

    public void Setup(int value, Color color)
    {
        tmp.SetText(value.ToString());
        tmp.color = color;
    }

    void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        transform.position += (Vector3) Vector2.up * 1f * Time.deltaTime;

        duration -= Time.deltaTime;
        if (duration < 0)
        {
            Destroy(gameObject);
        }
    }
}
