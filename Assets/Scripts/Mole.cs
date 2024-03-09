using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Mole : MonoBehaviour
{
    #region Values

    private bool isAttacking = false;

    public bool IsAttacking
    {
        get
        {
            return isAttacking;
        }
        set
        {
            isAttacking = value;
            Attack();
            //GeneratePositionOfAttack();
        }
    }


    public int PlayerEndViewValue = 8;
    public int RaiusOfAttack = 5;
    public bool IsBombed = false;

    public int MaxNumbreOfRootsToBeEaten = 3;

    public float Health = 100;

    public float SizeOfTile = -0.5f;

    [SerializeField]
    private bool findRoots = false;

    [SerializeField]
    private Vector2 attackingStartPosition;

    private GameEngine gameEnginInstance;

    private (int, int) lastEatenRoot;

    #endregion

    public Mole(int generatedAttackHeight, GameEngine engineInstance)
    {
        //GenerateAttack(generatedAttackHeight);
        PlayerEndViewValue = generatedAttackHeight;
        MoveToAttackPosition();
        gameEnginInstance = engineInstance;
        IsAttacking = true;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Attack()
    {
        int tmp_index = 0;
        foreach (var item in gameEnginInstance.map[PlayerEndViewValue])
        {
            if (item.HasRoots)
            {
                if (item.type == MapTileType.Toxin)
                {
                    IsBombed = true;
                    return;
                }
                else
                {
                    findRoots = true;
                    return;
                }
            }
            tmp_index++;
        }


        Vector3 endAttackPosition = new Vector3(tmp_index, attackingStartPosition.y);

        transform.DOMove(endAttackPosition, 1f).OnComplete(() =>
        {
            //MOVE TO ROOTS
            if (IsBombed)
            {
                IsBombed = false;
                Health -= 5;
                if (Health == 0)
                {
                    //TODO end mole
                    Debug.Log("HEY VICTORY!");
                }

                RunAway();

            }
            else if (findRoots)
            {
                EatRoots(tmp_index);
            }
            else
            {
                Debug.Log("SMTH went wrong. Did not find bomb nor roots.");
            }
        });



    }

    private void EatRoots(int xPos)
    {
        int numOfToBeEatenRoots = UnityEngine.Random.Range(0, MaxNumbreOfRootsToBeEaten);
        lastEatenRoot = (xPos, (int)attackingStartPosition.y);
        gameEnginInstance.RemoveRoots(lastEatenRoot.Item1, lastEatenRoot.Item2);
        for (int i = 0; i < numOfToBeEatenRoots; i++)
        {
            GenerteEatenRoot(lastEatenRoot);
            gameEnginInstance.RemoveRoots(lastEatenRoot.Item1, lastEatenRoot.Item2);

        }
    }

    private void GenerteEatenRoot((int, int) positionStart)
    {
        List<(int, int)> rootsPosition = new List<(int, int)>();
        if (gameEnginInstance.map[0].Length <= positionStart.Item2 - 1
            && gameEnginInstance.map[positionStart.Item1][positionStart.Item2 - 1].HasRoots)
        {
            rootsPosition.Add((positionStart.Item1, positionStart.Item2 - 1));
        }
        if (gameEnginInstance.map[0].Length >= positionStart.Item2 + 1 &&
            gameEnginInstance.map[positionStart.Item1][positionStart.Item2 + 1].HasRoots)
        {
            rootsPosition.Add((positionStart.Item1, positionStart.Item2 + 1));
        }
        if (gameEnginInstance.map.Count >= positionStart.Item1 + 1 &&
            gameEnginInstance.map[positionStart.Item1 + 1][positionStart.Item2].HasRoots)
        {
            rootsPosition.Add((positionStart.Item1 + 1, positionStart.Item2));
        }
        if (gameEnginInstance.map.Count <= positionStart.Item1 - 1 &&
            gameEnginInstance.map[positionStart.Item1 - 1][positionStart.Item2].HasRoots)
        {
            rootsPosition.Add((positionStart.Item1 - 1, positionStart.Item2));
        }

        int way = UnityEngine.Random.Range(0, rootsPosition.Count);

        lastEatenRoot = rootsPosition[way];
    }

    private void MoveToAttackPosition()
    {

        //HARDCODED X POSITION
        this.transform.position = new Vector3(-15f, -PlayerEndViewValue, 4f);
        attackingStartPosition = this.transform.position;

    }

    private void RunAway()
    {
        transform.DOShakePosition(1f, 0.05f, 10, 90f, false).OnComplete(() =>
        {
            transform.DOMove(attackingStartPosition, 0.5f);

        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
