using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBGMove : MonoBehaviour
{
    public Image BCB;
    public Image MCB;
    public Image FCB;

    public Image BBB;
    public Image MBB;
    public Image FBB;

    private float Bspeed = 0.01f;
    private float Mspeed = 0.02f;
    private float Fspeed = 0.03f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        BGScroll(BCB, Bspeed);
        BGScroll(MCB, -Mspeed);
        BGScroll(FCB, Fspeed);

        BGScroll(BBB, Bspeed);
        BGScroll(MBB, -Mspeed);
        BGScroll(FBB, Fspeed);
    }

    void BGScroll(Image texture, float Speed) // Material Offset값 변경
    {
        Vector2 textureOffset = new Vector2(Time.time * Speed, 0);
        texture.material.mainTextureOffset = textureOffset;
    }

    //void MoveCBOffset(Material mat, float speed)
    //{
    //    mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x + speed * Time.deltaTime, 0);
    //    if (mat.mainTextureOffset.x > 0.6f)
    //    {
    //        mat.mainTextureOffset = new Vector2(-0.896f, 0);
    //    }
    //}

    //void MoveBBOffset(Material mat, float speed)
    //{
    //    mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x + speed * Time.deltaTime, 0);
    //    if (mat.mainTextureOffset.x > 0.82f)
    //    {
    //        mat.mainTextureOffset = new Vector2(-0.896f, 0);
    //    }
    //}
}
