using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int widht, height;
    public Brick cube;
    public ColorData colorData;
    public Transform baseTranform;
    private Vector3 origin;
    private List<Brick> bricks = new List<Brick>();

    // Start is called before the first frame update
    void Start()
    {
        if (cube == null)
        {
            return;
        }

        if (colorData == null)
        {
            return;
        }
        SpawBricks();
    }

    public void SpawBricks()
    {
        for (int i = -5; i < widht; i++)
        {
            for (int j = 0; j < height; j++)
            {
                origin = new Vector3(baseTranform.position.x + i, baseTranform.position.y, baseTranform.position.z + j);
                Brick obj = SimplePool.Spawn<Brick>(cube, origin, baseTranform.rotation);
                if (obj != null)
                {
                    bricks.Add(obj);
                    obj.SetParentBrick(transform);
                    ColorEnum randomColor = (ColorEnum)Random.Range(1, System.Enum.GetValues(typeof(ColorEnum)).Length);
                    obj.brickColor = randomColor;
                    obj.SetColor(randomColor);
                    obj.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ActivateAllBricks()
    {
        foreach (Brick brick in bricks)
        {
            if (!brick.gameObject.activeSelf)
            {
                brick.gameObject.SetActive(true);
            }
        }
    }

    public List<Brick> GetListBrick()
    {
        return bricks;
    }
}
