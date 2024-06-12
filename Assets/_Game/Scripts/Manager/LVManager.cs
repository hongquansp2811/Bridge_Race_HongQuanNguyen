using System.Collections.Generic;
using UnityEngine;

public class Lvmanager : Singleton<Lvmanager>
{
    public ColorData colorData;
    public List<GameObject> maps; // List các Map có sẵn
    public CameraFollow cameraFollow; // Tham chiếu đến script CameraFollow

    public int currentMapIndex = 0;

    void Start()
    {
        LoadMap(0);
    }

    public void LoadNextLevel()
    {
        LoadMap((currentMapIndex + 1) % maps.Count);
    }

    public void ReplayCurrentLevel()
    {
        ResetCurrentMap();
    }

    public void LoadMap(int mapIndex)
    {
        if (mapIndex < 0 || mapIndex >= maps.Count)
        {
            return;
        }

        // Vô hiệu hóa map hiện tại
        if (currentMapIndex < maps.Count)
        {
            maps[currentMapIndex].SetActive(false);
        }

        // Kích hoạt map mới
        currentMapIndex = mapIndex;
        maps[currentMapIndex].SetActive(true);
        maps[currentMapIndex].GetComponent<Map>().UpdateCharacterInfo();

        // Thiết lập CameraFollow để follow Player của map hiện tại
        Transform player = maps[currentMapIndex].transform.Find("Player");
        if (player != null)
        {
            cameraFollow.SetTarget(player);
        }
        else
        {
            Debug.LogError("Player not found in the current map");
        }
    }

    private void ResetCurrentMap()
    {
        if (maps[currentMapIndex] != null)
        {
            maps[currentMapIndex].SetActive(false);
            maps[currentMapIndex].SetActive(true);
            maps[currentMapIndex].GetComponent<Map>().ResetCurrentMap();
        }
    }
}
