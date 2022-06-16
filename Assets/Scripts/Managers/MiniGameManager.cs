using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Pool;
using TMPro;

[System.Serializable]
public class StartMinigameEvent : UnityEvent
{

}

public class MiniGameManager : MonoBehaviour
{
    public static StartMinigameEvent startMinigameCountdownEvent;

    [SerializeField]
    private RectTransform MinigameRulesPanel;
    [SerializeField]
    private TextMeshProUGUI countdownText;
    [SerializeField]
    private Transform tilesReference;
    [SerializeField]
    private float minInterval;
    [SerializeField]
    private float maxInterval;

    [SerializeField]
    private Projectile projectilePrefab;
    private int spawnAmount;
    [SerializeField]
    private int defaultPoolCapacity;
    [SerializeField]
    private int maximumPoolCapacity;
    [SerializeField]
    private float projectileHeight;

    private bool gameFinished { get; set; }
    private float timeBeforeEventTrigger;
    private int selectedTile;
    private List<Tile> tilesList;
    private ObjectPool<Projectile> projectilePool;

    private void OnEnable()
    {
        gameFinished = false;
        if(startMinigameCountdownEvent == null)
        {
            startMinigameCountdownEvent = new StartMinigameEvent();
            startMinigameCountdownEvent.AddListener(StartCountdown);
        }
        GetTiles();
        CreateProjectilePool();
        PlayerJoinManager.positionPlayersEvent.Invoke();
    }
    private void OnDisable()
    {
        startMinigameCountdownEvent.RemoveAllListeners();
    }
    private void CreateProjectilePool()
    {
        spawnAmount = tilesList.Count;
        projectilePool = new ObjectPool<Projectile>(() =>
        {
            return Instantiate(projectilePrefab);
        }, projectile =>
        {
            projectile.gameObject.SetActive(true);
        }, projectile =>
        {
            projectile.gameObject.SetActive(false);
        }, projectile =>
        {
            Destroy(projectile.gameObject);
        }, false, defaultPoolCapacity, maximumPoolCapacity);
        //StartCoroutine(ProjectileSystem());
    }

    private void GetTiles()
    {
        tilesList = new List<Tile>();
        foreach(Tile tile in tilesReference.GetComponentsInChildren<Tile>())
        {
            tilesList.Add(tile);
        }
        //StartCoroutine(TileSystem());
    }

    private void ReleaseProjectile(Projectile projectile)
    {
        projectilePool.Release(projectile);
    }

    private void SetupProjectiles()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Projectile projectile = projectilePool.Get();
            projectile.transform.parent = tilesList[i].transform;
            projectile.transform.position = new Vector3(tilesList[i].transform.position.x, tilesList[i].transform.position.y + projectileHeight, tilesList[i].transform.position.z);
            projectile.setReleaseAction(ReleaseProjectile);
            projectile.gameObject.SetActive(false);
        }
    }

    public void StartCountdown()
    {
        MinigameRulesPanel.gameObject.SetActive(false);
        StartCoroutine(Countdown());
    }

    IEnumerator TileSystem()
    {
        while (!gameFinished)
        {
            timeBeforeEventTrigger = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(timeBeforeEventTrigger);
            selectedTile = Random.Range(0, tilesList.Count - 1);
            if (tilesList[selectedTile].isTargetable)
            {
                tilesList[selectedTile].isTargetable = false;
                tilesList[selectedTile].StartShaking();
            }
        }
    }
    IEnumerator ProjectileSystem()
    {
        SetupProjectiles();
        while (!gameFinished)
        {
            timeBeforeEventTrigger = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(timeBeforeEventTrigger);
            selectedTile = Random.Range(0, projectilePool.CountAll);
            if (tilesList[selectedTile].isTargetable)
            {
                tilesList[selectedTile].isTargetable = false;
                int projectile = tilesList[selectedTile].transform.childCount - 1;
                tilesList[selectedTile].transform.GetChild(projectile).gameObject.SetActive(true);
            }
        }
    }

    IEnumerator Countdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.text = "Go";
        PlayerJoinManager.switchControlsEvent.Invoke();
        StartCoroutine(TileSystem());
        StartCoroutine(ProjectileSystem());
        yield return new WaitForSeconds(1);
        countdownText.text = "";
        StopCoroutine(Countdown());
    }
}
