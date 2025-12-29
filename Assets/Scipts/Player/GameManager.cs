using System.Collections;
using NUnit.Framework.Constraints;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Gameplay;
    [SerializeField] public GameObject up;
    [SerializeField] public GameObject down;
    public static GameManager Instance { get; private set; }
    public HUD hud;
    public MovementPlayer player;
    public PlayerStateList playerState;
    public BossesCount bossCount;
    [SerializeField] public deathScreen deathScreen;
    
    public const int MAX_HEALTH = 10;
    public int _currentHealth;
    private Vector2 respawnPoint;
    public string transitionedfromscene;
    public Vector2 exitDirection;
    public bool LlaveDave = false;
    public CinemachineCamera NormalCamera;
    public CinemachineCamera LookDownCamera;

    public string actualScene;
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _currentHealth = MAX_HEALTH;
            hud.UpdateHearts(_currentHealth);
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if ((player.isGrounded || player.Platform()) && Input.GetAxisRaw("Vertical") < 0 && playerState.canMove)
        {
            NormalCamera.Priority = 10;
            LookDownCamera.Priority = 11;
        }
        else
        {
            NormalCamera.Priority = 10;
            LookDownCamera.Priority = 5;
        }
    }

    public void save()
    {
        actualScene = SceneManager.GetActiveScene().name;
        SaveManager.savePlayerData(this);
        Debug.Log("Game Saved");
    }

    public void load()
    {
        SaveFile save = SaveManager.loadPlayerData();
        _currentHealth = save.health;
        hud.UpdateHearts(_currentHealth);
        LlaveDave = save.llaveDave;
        playerState.ableToDash = save.ableToDash;
        bossCount.RoyalGuardAlive = save.RoyalGuardAlive;
        bossCount.killCount = save.killCount;
        actualScene = save.actualScene;
        player.transform.position = new Vector3(save.position[0], save.position[1]);
        Debug.Log("Game Loaded");
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, MAX_HEALTH);
        hud.UpdateHearts(_currentHealth);

        if (_currentHealth <= 0 && !player.isDead)
        {
            player.pState.canMove = false;
            Time.timeScale = 0.5f;
            player.TriggerDeathAnimation();
            StartCoroutine(deathTime());
        }
    }
    public IEnumerator deathTime()
    {
        yield return new WaitForSecondsRealtime(player.deathAnimationDuration + 0.5f);
        Time.timeScale = 1f;
        player.pState.canMove = true;
        deathScreen.deploy();
    }

    


    public void Heal(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, MAX_HEALTH); //me calcula la vida maxima del jugador y le agrega una cantidad x de corazones hasta maxHealth
        hud.UpdateHearts(_currentHealth);
    }
    public void SetRespawnPoint(Vector2 position)
    {
        respawnPoint = position;
    }

    public Vector2 GetRespawnPoint()
    {
        return respawnPoint;
    }
}