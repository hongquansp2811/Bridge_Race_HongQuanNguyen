using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Charater
{
    public NavMeshAgent agent;
    private Transform targetBrick;
    public Transform FinnalPos;
    private List<Transform> brickTranforms;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        OnInit();
    }

    public void OnInit()
    {
        StartCoroutine(BrickRoutine());
        curentPlatform = map.platforms[0];
        UpdateBrickTransforms();
    }

    private IEnumerator BrickRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        brickTranforms = GetListBrickTranform();

        while (true)
        {
            // Nếu có ít hơn 5 viên gạch trên lưng, tìm và di chuyển đến viên gạch
            while (bricksOnBack.Count < 10)
            {
                targetBrick = FindNearestBrick();
                if (targetBrick != null)
                {
                    agent.SetDestination(targetBrick.transform.position);
                    anim.SetBool("Running", true);

                    // Chờ cho đến khi đến gần viên gạch hoặc nó không còn tồn tại
                    while (targetBrick != null && Vector3.Distance(transform.position, targetBrick.transform.position) > 1f)
                    {
                        yield return null;
                    }
                }
                yield return new WaitForSeconds(0.5f);
            }

            // Khi có đủ 5 viên gạch, di chuyển đến cầu
            MoveToBridge();
            while (bricksOnBack.Count > 0)
            {
                // Đợi một khoảng thời gian để mô phỏng việc xây cầu
                yield return new WaitForSeconds(0.5f);
            }

            anim.SetBool("Running", false);
        }
    }

    private void MoveToBridge()
    {
        if (FinnalPos != null)
        {
            agent.SetDestination(FinnalPos.position);
            anim.SetBool("Running", true);
        }
    }

    private Transform FindNearestBrick()
    {
        Transform nearestBrick = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform brickTranform in brickTranforms)
        {
            if (brickTranform.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(transform.position, brickTranform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestBrick = brickTranform;
                }
            }
        }
        return nearestBrick;
    }

    private List<Transform> GetListBrickTranform()
    {
        List<Brick> bricks = curentPlatform.GetListBrick().Where(x => x.brickColor == colorEnum).ToList();
        List<Transform> list = new List<Transform>();
        for(int i = 0; i < bricks.Count; i++)
        {
            list.Add(bricks[i].transform);
        }
        return list;
    }

    public void UpdateBrickTransforms()
    {
        if (curentPlatform != null)
        {
            List<Brick> bricks = curentPlatform.GetListBrick().Where(x => x.brickColor == colorEnum).ToList();
            brickTranforms = new List<Transform>();
            foreach (Brick brick in bricks)
            {
                brickTranforms.Add(brick.transform);
            }
        }
    }

    public void RestartMovement()
    {
        Debug.Log("RestartMovement called for " + gameObject.name);

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Wait for the next frame to ensure agent is properly placed on NavMesh
        StartCoroutine(RestartMovementCoroutine());
    }

    private IEnumerator RestartMovementCoroutine()
    {
        // Wait until the next frame to ensure agent is properly placed on NavMesh
        yield return null;

        // Reset agent state
        agent.isStopped = false;

        // Restart the BrickRoutine coroutine
        StopAllCoroutines();
        StartCoroutine(BrickRoutine());
    }

    protected override void HandleFinnishCollision()
    {
        // Hiển thị GameOverMenu khi Enemy va chạm với Finnish
        GameManager.Instance.GameOver();
    }
}
