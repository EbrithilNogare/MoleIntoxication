using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Mole
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
    public bool IsToxicated = false;

    public int MaxNumbreOfRootsToBeEaten = 3;

    public float Health = 100;

    public float SizeOfTile = -0.5f;

    private bool findRoots;

    [SerializeField]
    private Vector2 attackingStartPosition = new Vector2();

    private GameEngine gameEnginInstance;
    private Transform moleTransform;

    private (int, int) lastEatenRoot;

    #endregion

    public Mole(int generatedAttackHeight, GameEngine engineInstance, Transform moleGO)
    {
        //GenerateAttack(generatedAttackHeight);
        PlayerEndViewValue = generatedAttackHeight;
        moleTransform = moleGO;
        MoveToAttackPosition();
        gameEnginInstance = engineInstance;
        findRoots = false;
        IsBombed = false;
        IsToxicated = false;
        IsAttacking = true;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Attack()
    {
        int tmp_index = 0;
        int important_index = -1;
        foreach (var item in gameEnginInstance.map[PlayerEndViewValue])
        {
            if (item.HasRoots)
            {
                if (item.type == MapTileType.Toxin)
                {
                    IsBombed = true;
                    if (important_index == -1)
                        important_index = tmp_index;
                    break;
                }
                else
                {
                    findRoots = true;
                    if (important_index == -1)
                        important_index = tmp_index;
                    break;
                }
            }
            tmp_index++;
        }

        Debug.Log("Bomba: " + IsBombed + "....." + "Root: " + findRoots);

        Vector3 endAttackPosition = new Vector3(important_index - 7, attackingStartPosition.y);
        Debug.Log("GO: " + endAttackPosition.x + "....." + "Y: " + endAttackPosition.y);

        moleTransform.DOMove(endAttackPosition, 1f).OnComplete(() =>
        {
            //MOVE TO ROOTS
            //Debug.Log("Bomba: " + IsBombed + "....." + "Root: " + findRoots);
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
                EatRoots(important_index);
            }
            else
            {
                Debug.Log("SMTH went wrong. Did not find bomb nor roots.");
            }
        });



    }

    private void EatRoots(int xPos)
    {
        Debug.Log("tu");
        int numOfToBeEatenRoots = UnityEngine.Random.Range(1, MaxNumbreOfRootsToBeEaten);
        lastEatenRoot = (xPos, -(int)attackingStartPosition.y);
        gameEnginInstance.RemoveRoots(lastEatenRoot.Item1, lastEatenRoot.Item2);
        Debug.Log("MNAM:" + lastEatenRoot.Item1 + "..." + lastEatenRoot.Item2);
        for (int i = 0; i < numOfToBeEatenRoots; i++)
        {
            GenerteEatenRoot(lastEatenRoot);
            if (gameEnginInstance.map[lastEatenRoot.Item2][lastEatenRoot.Item1].type != MapTileType.Toxin)
            {
                Vector3 endAttackPosition = new Vector3(lastEatenRoot.Item1 - 7, -lastEatenRoot.Item2);
                moleTransform.DOMove(endAttackPosition, 1f).OnComplete(() =>
                {
                    gameEnginInstance.RemoveRoots(lastEatenRoot.Item1, lastEatenRoot.Item2);
                    Debug.Log("MNAM:" + lastEatenRoot.Item1 + "..." + lastEatenRoot.Item2);

                });
            }
            else
            {
                RunAway();
                return;
            }


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
        attackingStartPosition = new Vector2(lastEatenRoot.Item1 - 7, -lastEatenRoot.Item2);
    }

    private void MoveToAttackPosition()
    {
        //HARDCODED X POSITION
        moleTransform.position = new Vector3(-15f, -PlayerEndViewValue, 4f);
        attackingStartPosition = moleTransform.position;

    }

    private void RunAway()
    {
        moleTransform.DOShakePosition(1f, 0.05f, 10, 90f, false).OnComplete(() =>
        {
            moleTransform.DOMove(attackingStartPosition, 0.5f);

        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
