using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public int MaxNumbreOfRootsToBeEaten = 5;
    public int numOfToBeEatenRoots = 0;

    public int Health = 100;

    public float SizeOfTile = -0.5f;

    private bool findRoots;

    [SerializeField]
    private Vector2 attackingStartPosition = new Vector2();

    private GameEngine gameEnginInstance;
    private Transform moleTransform;
    private Transform healthBar;

    private (int, int) lastEatenRoot;

    private System.Action<int> healthUpdate;
    private System.Action returnTurn;

    #endregion

    public Mole(int generatedAttackHeight, GameEngine engineInstance, Transform moleGO, Transform healthBarT, int _health, System.Action<int> healthUpdate, System.Action returnTurn, int MaxNumbreOfRootsToBeEaten)
    {
        //GenerateAttack(generatedAttackHeight);
        this.MaxNumbreOfRootsToBeEaten = MaxNumbreOfRootsToBeEaten;
        this.returnTurn = returnTurn;
        PlayerEndViewValue = generatedAttackHeight;
        moleTransform = moleGO;
        healthBar = healthBarT;
        Health = _health;
        MoveToAttackPosition();
        gameEnginInstance = engineInstance;
        findRoots = false;
        IsBombed = false;
        IsToxicated = false;
        IsAttacking = true;
        this.healthUpdate = healthUpdate;
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
                healthBar.GetChild((int)Mathf.Abs((Health / 20) - 5)).gameObject.SetActive(false);
                Health -= 20;
                lastEatenRoot = (important_index, -(int)attackingStartPosition.y);
                SFXManager.Instance.PlayToxinPop();

                gameEnginInstance.map[lastEatenRoot.Item2][lastEatenRoot.Item1].type = MapTileType.Empty;
                healthUpdate(Health);
                healthBar.GetChild((int)Mathf.Abs((Health / 20) - 5)).gameObject.SetActive(true);
                if (Health == 0)
                {
                    Debug.Log("HEY VICTORY!");
                    //TODO end mole
                    SceneManager.LoadScene(4);
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
        numOfToBeEatenRoots = UnityEngine.Random.Range(0, MaxNumbreOfRootsToBeEaten);
        lastEatenRoot = (xPos, -(int)attackingStartPosition.y);
        gameEnginInstance.RemoveRoots(lastEatenRoot.Item1, lastEatenRoot.Item2);
        SFXManager.Instance.PlayMole();
        Debug.Log("MNAM:" + numOfToBeEatenRoots + "x ... AT" + lastEatenRoot.Item1 + "..." + lastEatenRoot.Item2);
        //for (int i = 0; i < numOfToBeEatenRoots; i++)
        //{
        if (numOfToBeEatenRoots > 0)
        {
            TweenToEat();

        }
        else
        {
            Debug.Log("Run home");
            Vector2 direction2 = new Vector2(-15 - lastEatenRoot.Item1, lastEatenRoot.Item2);
            float angleRadians2 = Mathf.Atan2(direction2.y, direction2.x);
            float angleDegrees2 = angleRadians2 * Mathf.Rad2Deg;
            moleTransform.DOLocalRotate(new Vector3(0, 0, angleDegrees2), 0.5f).OnComplete(() =>
            {
                moleTransform.DOMoveX(-15f, 1f).OnComplete(() =>
                {
                    moleTransform.DOLocalRotate(new Vector3(0, 0, 0), 0f).OnComplete(() => returnTurn());
                });

            });
        }


    }

    private void TweenToEat()
    {
        Vector2 tmp = new Vector2(lastEatenRoot.Item1 - 7, -lastEatenRoot.Item2);
        GenerteEatenRoot(lastEatenRoot);
        Vector2 direction = new Vector2((lastEatenRoot.Item1 - 7) - tmp.x, -(lastEatenRoot.Item2) - tmp.y);
        float angleRadians = Mathf.Atan2(direction.y, direction.x);
        float angleDegrees = angleRadians * Mathf.Rad2Deg;
        if (gameEnginInstance.map[lastEatenRoot.Item2][lastEatenRoot.Item1].type != MapTileType.Toxin)
        {
            moleTransform.DOLocalRotate(new Vector3(0, 0, angleDegrees), 1f);

            Vector3 endAttackPosition = new Vector3(lastEatenRoot.Item1 - 7, -lastEatenRoot.Item2, 0);

            moleTransform.DOMove(endAttackPosition, 1f).OnComplete(() =>
            {
                gameEnginInstance.RemoveRoots(lastEatenRoot.Item1, lastEatenRoot.Item2);
                SFXManager.Instance.PlayMole();

                Debug.Log("MNAM:" + lastEatenRoot.Item1 + "..." + lastEatenRoot.Item2);
                numOfToBeEatenRoots--;
                if (numOfToBeEatenRoots == 0)
                {
                    Debug.Log("Run home");
                    Vector2 direction2 = new Vector2(-15 - lastEatenRoot.Item1, lastEatenRoot.Item2);
                    float angleRadians2 = Mathf.Atan2(direction2.y, direction2.x);
                    float angleDegrees2 = angleRadians2 * Mathf.Rad2Deg;
                    moleTransform.DOLocalRotate(new Vector3(0, 0, angleDegrees2), 0.5f).OnComplete(() =>
                    {
                        moleTransform.DOMoveX(-15f, 1f).OnComplete(() =>
                        {
                            moleTransform.DOLocalRotate(new Vector3(0, 0, 0), 0f).OnComplete(() => returnTurn());
                        });

                    });

                }
                else
                {
                    TweenToEat();

                }

            });
        }
        else
        {
            healthBar.GetChild((int)Mathf.Abs((Health / 20) - 5)).gameObject.SetActive(false);
            Health -= 20;
            SFXManager.Instance.PlayToxinPop();

            gameEnginInstance.map[lastEatenRoot.Item2][lastEatenRoot.Item1].type = MapTileType.Empty;
            healthUpdate(Health);
            healthBar.GetChild((int)Mathf.Abs((Health / 20) - 5)).gameObject.SetActive(true);
            if (Health == 0)
            {
                Debug.Log("HEY VICTORY!");
                //TODO end mole
                SceneManager.LoadScene(4);
            }
            RunAway();
            return;
        }
    }

    private void GenerteEatenRoot((int, int) positionStart)
    {
        List<(int, int)> rootsPosition = new List<(int, int)>();

        if (0 <= positionStart.Item1 - 1 && gameEnginInstance.map[positionStart.Item2][positionStart.Item1 - 1].HasRoots)
        {
            rootsPosition.Add((positionStart.Item1 - 1, positionStart.Item2));
        }
        if (gameEnginInstance.map[0].Length > positionStart.Item1 + 1 && gameEnginInstance.map[positionStart.Item2][positionStart.Item1 + 1].HasRoots)
        {
            rootsPosition.Add((positionStart.Item1 + 1, positionStart.Item2));
        }
        if (gameEnginInstance.map.Count > positionStart.Item2 + 1 && gameEnginInstance.map[positionStart.Item2 + 1][positionStart.Item1].HasRoots)
        {
            rootsPosition.Add((positionStart.Item1, positionStart.Item2 + 1));
        }
        if (0 <= positionStart.Item2 - 1 && gameEnginInstance.map[positionStart.Item2 - 1][positionStart.Item1].HasRoots)
        {
            rootsPosition.Add((positionStart.Item1, positionStart.Item2 - 1));
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
        Vector2 direction = new Vector2(-15 - lastEatenRoot.Item1, lastEatenRoot.Item2);
        float angleRadians = Mathf.Atan2(direction.y, direction.x);
        float angleDegrees = angleRadians * Mathf.Rad2Deg;
        moleTransform.DOShakePosition(1f, 0.05f, 10, 90f, false).OnComplete(() =>
        {
            moleTransform.DOLocalRotate(new Vector3(0, 0, angleDegrees), 0.5f).OnComplete(() =>
            {
                moleTransform.DOMoveX(-15f, 1f).OnComplete(() =>
                {
                    moleTransform.DOLocalRotate(new Vector3(0, 0, 0), 0f).OnComplete(() => returnTurn());
                });

            });

        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
