using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.StickyNote;

public class Brick : GameUnit
{
    public ColorEnum brickColor;

    public void SetPosition(Vector3 position)
    {
        TF.localPosition = position;
    }

    public void SetParentBrick(Transform parrent)
    {
        TF.SetParent(parrent);
    }

    public void SetColor(ColorEnum color)
    {
        brickColor = color;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material= Lvmanager.Instance.colorData.GetColorData(color);
        }
        else
        {
            Debug.LogError("Renderer not found on Brick.");
        }
    }

    public void SetBrickInBack(Vector3 position, ColorEnum colorEnum, Transform backPos)
    {
        this.GetComponent<Collider>().isTrigger = false;
        SetColor(colorEnum);
        SetParentBrick(backPos);
        SetPosition(position);
    }
}
