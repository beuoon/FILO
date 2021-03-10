using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DisasterObject : MonoBehaviour {
    protected Sprite[] sprites;
    protected float frameInterval;

    private SpriteRenderer spriteRenderer;
    private float accumTime = 0;
    private int spriteIndex = 0;

    // Start is called before the first frame update
    protected virtual void Start() {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update() {
        accumTime += Time.deltaTime;
        spriteIndex = Mathf.FloorToInt(accumTime / frameInterval);

        if (spriteIndex >= sprites.Length) {
            Active();
            Destroy(this.gameObject);
        }
        else
            spriteRenderer.sprite = sprites[spriteIndex];
    }

    protected abstract void Active();
}
