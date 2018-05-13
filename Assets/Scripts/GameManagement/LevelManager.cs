using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(WaveManager))]
public class LevelManager : MonoBehaviour
{
    [SerializeField] bool bossLevel = false;
    [SerializeField] GameObject boss;
    [SerializeField] private Slider bossSlider;
    [SerializeField] private Text bossText;
    [SerializeField] private AudioSource bossMusicSource;
    [SerializeField] private AudioClip bossFinalPhaseClip;

    private int bossPhase = 0;
    EnemyHealth bossHealth;

    private GameManager gameManager;
    private WaveManager waveManager;
    private PlayerHealth playerHealth;

    private float waveSpawnDelay = 3.0f;
    private bool gameOver = false;
    private bool victory = false;

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
        waveManager = GetComponent<WaveManager>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        if (bossLevel)
        {
            bossHealth = boss.GetComponent<EnemyHealth>();
            bossText.text = "Phase 1 / 3";
            bossSlider.maxValue = 750;
        }
    }

    void Update()
    {
        if (gameOver || victory)
        {
            return;
        }

        if (playerHealth.IsDead())
        {
            gameOver = true;
            Invoke("TriggerGameOver", 5.0f);
        }

        if (!bossLevel)
        {
            if (!waveManager.IsFirstWaveSpawned())
            {
                waveManager.SpawnNextWave();
            }

            if (waveManager.IsCurrentWaveDefeated())
            {
                if (waveManager.AreAllWavesDefeated())
                {
                    victory = true;
                    DespawnEnemies();
                    Invoke("TriggerVictory", 3.0f);
                }
                else
                {
                    waveManager.SpawnNextWave(waveSpawnDelay);
                }
            }
        }
        else
        {
            if (bossPhase == 0)
            {
                bossSlider.value = bossHealth.GetCurrentHealth() - 1250;

                if (bossHealth.GetCurrentHealth() <= 1250)
                {
                    bossPhase = 1;
                    bossSlider.maxValue = 750;
                    bossText.text = "Phase 2 / 3";
                    boss.GetComponent<Troll>().TriggerPhase2();
                }
            }
            else if (bossPhase == 1)
            {
                bossSlider.value = bossHealth.GetCurrentHealth() - 500;

                if (bossHealth.GetCurrentHealth() <= 500)
                {
                    bossMusicSource.clip = bossFinalPhaseClip;
                    bossMusicSource.Play();
                    bossPhase = 2;
                    bossSlider.maxValue = 500;
                    bossText.text = "Phase 3 / 3";
                    boss.GetComponent<Troll>().TriggerPhase3();
                }
            }
            else
            {
                bossSlider.value = bossHealth.GetCurrentHealth();
            }

            if (bossHealth.IsDead())
            {
                victory = true;
                DespawnEnemies();
                Invoke("TriggerVictory", 5.0f);
            }
        }
    }

    public bool IsLevelActive()
    {
        return !gameOver && !victory;
    }

    public void TriggerGameOver()
    {
        gameManager.TriggerGameOver();
    }

    public void TriggerVictory()
    {
        gameManager.TriggerVictory();
    }

    private void DespawnEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && !enemies[i].GetComponent<EnemyHealth>().IsDead() && enemies[i].GetComponent<EnemyHealth>().IsDestroyedOnVictory())
            {
                Destroy(enemies[i]);
            }
        }
    }
}
