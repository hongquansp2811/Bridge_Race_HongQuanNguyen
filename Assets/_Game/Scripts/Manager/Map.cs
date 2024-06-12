using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform[] startPositions; // Các vị trí bắt đầu cho Player và Enemy
    public Charater player;
    public Enemy[] enemies;
    public List<Platform> platforms;
    public List<GameObject> bridges;

    private List<ColorEnum> availableColors;
    private List<ColorEnum> assignedColors;

    public void Awake()
    {
        InitializeColors();
    }

    void Start()
    {
        Debug.Log("Map Start: " + gameObject.name);
    }

    private void InitializeColors()
    {
        availableColors = new List<ColorEnum>();
        assignedColors = new List<ColorEnum>();

        foreach (ColorEnum color in System.Enum.GetValues(typeof(ColorEnum)))
        {
            if (color != ColorEnum.None)
            {
                availableColors.Add(color);
            }
        }
    }

    public void SetInitialPositions()
    {
        if (startPositions.Length >= 1)
        {
            player.TF.position = startPositions[0].position;

            for (int i = 0; i < enemies.Length && i < startPositions.Length - 1; i++)
            {
                enemies[i].TF.position = startPositions[i + 1].position;
            }
        }
        else
        {
            Debug.LogWarning("Not enough start positions assigned.");
        }
    }

    private void AssignRandomColors()
    {
        InitializeColors();
        // Đặt màu ngẫu nhiên cho player
        if (player != null)
        {
            ColorEnum colorEnumP = GetRandomColor();
            player.colorEnum = colorEnumP;
            SkinnedMeshRenderer[] meshRenderers = player.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer renderer in meshRenderers)
            {
                renderer.material = Lvmanager.Instance.colorData.GetColorData(colorEnumP);
            }
        }

        // Đặt màu ngẫu nhiên cho các enemies
        foreach (Charater enemy in enemies)
        {
            if (enemy != null)
            {
                ColorEnum colorEnumE = GetRandomColor();
                enemy.ChangColor(colorEnumE);
            }
        }
    }

    public ColorEnum GetRandomColor()
    {
        if (availableColors.Count == 0)
        {
            return ColorEnum.None;
        }

        int randomIndex = Random.Range(0, availableColors.Count);
        ColorEnum selectedColor = availableColors[randomIndex];
        availableColors.RemoveAt(randomIndex);
        assignedColors.Add(selectedColor);
        return selectedColor;
    }

    public void ReleaseColor(ColorEnum color)
    {
        if (assignedColors.Contains(color))
        {
            assignedColors.Remove(color);
            availableColors.Add(color);
        }
    }

    public void ResetBridge()
    {
        foreach (var bridge in bridges)
        {
            foreach (Transform child in bridge.transform)
            {
                Brick brick = child.GetComponent<Brick>();
                if (brick != null)
                {
                    brick.SetColor(ColorEnum.None);
                }
            }
        }
    }

    public void UpdateCharacterInfo()
    {
        SetInitialPositions();
        AssignRandomColors();
    }

    public void ResetCurrentMap()
    {
        UpdateCharacterInfo();
        for (int i = 0; i < platforms.Count; i++)
        {
            platforms[i].ActivateAllBricks();
        }
        player.DelAllBrickOnBack();
        foreach (var enemy in enemies)
        {
            enemy.DelAllBrickOnBack();
            enemy.RestartMovement();
        }

        //Khôi phục Bridge
        ResetBridge();
    }
}
