using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event_FallingRock : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private const int FRAME_INTERVAL = 10;
    private int frameCount = 0;
    private int spriteIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (++frameCount % FRAME_INTERVAL == 0) {
            if (++spriteIndex > sprites.Length) {
                Active();
                Destroy(this.gameObject);
            }

            spriteRenderer.sprite = sprites[spriteIndex];
            frameCount = 0;
		}
    }

    protected abstract void Active();
}
