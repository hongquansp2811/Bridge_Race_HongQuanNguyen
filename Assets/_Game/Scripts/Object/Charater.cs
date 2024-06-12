using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.StickyNote;

public class Charater : GameUnit
{
    public Transform backPos;
    public ColorEnum colorEnum;
    public bool isMoveUp = true;
    public Animator anim;
    public Stack<Brick> bricksOnBack = new Stack<Brick>();
    public Platform curentPlatform;
    public Map map;
    private float yPos = 1f;


    private void OnTriggerEnter(Collider other)
    {
        ColliderWithBrick(other);
        ColliderWithBridge(other);
        ColliderWithFinnish(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ColliderExitBridge(other);
        ColliderExitDoor(other);
    }
    
    private void ColliderWithBrick(Collider other)
    {
        if (!other.gameObject.CompareTag(Cache.CACHE_TAG_BRICK)) return;
        Brick brick = Cache.GetComponentFromCache<Brick>(other);
        if (brick == null || this.colorEnum != brick.brickColor) return;
        AddBack(other.gameObject);
        StartCoroutine(RespawnBrick(brick.TF.position, brick.brickColor, 20f));
    }

    private void ColliderWithBridge(Collider other)
    {
        if (!other.gameObject.CompareTag(Cache.CACHE_TAG_BRIDGE)) return;
        Brick brick = Cache.GetComponentFromCache<Brick>(other);
        if (brick == null) return;
        if (bricksOnBack.Count > 0)
        {
            if (this.colorEnum != brick.brickColor)
            {
                Brick topBrick = bricksOnBack.Pop();
                Destroy(topBrick.gameObject);
                yPos -= 0.25f;
                brick.SetColor(colorEnum);
            }
        }
        else
        {
            if (this.colorEnum != brick.brickColor)
            {
                isMoveUp = false;
            }
        }
    }

    private void ColliderWithFinnish(Collider other)
    {
        if (!other.gameObject.CompareTag(Cache.CACHE_TAG_FINNISH)) return;
        Debug.Log("Finnishhhhhhhhhhhhhhhhhhhhhhh");
        if (gameObject.CompareTag("Player"))
        {

        }
        HandleFinnishCollision();
    }

    private void ColliderExitBridge(Collider other)
    {
        if (!other.gameObject.CompareTag(Cache.CACHE_TAG_BRIDGE)) return;
        isMoveUp = true;
    }

   
    private void ColliderExitDoor(Collider other)
    {
        if (!other.gameObject.CompareTag(Cache.CACHE_TAG_DOOR)) return;
        other.GetComponent<Collider>().isTrigger = false;
        curentPlatform = map.platforms[1];
        Debug.Log("Qua cuwar....");
    }

    public void AddBack(GameObject obj)
    {
        obj.SetActive(false);
        Brick brick = SimplePool.Spawn<Brick>(PoolType.Brick, backPos.position, backPos.rotation);
        if (brick == null) return;
        brick.SetBrickInBack(new Vector3(0, yPos, 0), colorEnum, backPos);
        bricksOnBack.Push(brick);
        yPos += 0.25f;
    }

    public void DelAllBrickOnBack()
    {
        foreach (Brick brick in bricksOnBack)
        {
            Destroy(brick.gameObject);
        }
        bricksOnBack.Clear();

        // Reset lại vị trí yPos
        yPos = 1f;
    }

    private IEnumerator RespawnBrick(Vector3 originalPosition, ColorEnum brickColor, float delay)
    {
        yield return new WaitForSeconds(delay);

        Brick respawnedBrick = SimplePool.Spawn<Brick>(PoolType.Brick, originalPosition, Quaternion.identity);
        if (respawnedBrick != null)
        {
            respawnedBrick.brickColor = brickColor;
            respawnedBrick.SetColor(brickColor);
        }
    }

    protected virtual void HandleFinnishCollision()
    {
    }

    public void ChangColor(ColorEnum color)
    {
        this.colorEnum = color;
        SkinnedMeshRenderer[] meshRenderers = this.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in meshRenderers)
        {
            renderer.material = Lvmanager.Instance.colorData.GetColorData(color);
        }
    }
}
