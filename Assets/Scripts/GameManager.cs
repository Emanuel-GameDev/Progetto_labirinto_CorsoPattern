using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Setup,
    Spawn,
    HeroTurn,
    EnemyState
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GridManager gridManager;
    public StateMachine<GameState> StateMachine { get; } = new();


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        gridManager = transform.GetChild(0).GetComponent<GridManager>();    
    }

    private void Start()
    {
        StateMachine.RegisterState(GameState.Setup, new SetupState(this));
        StateMachine.RegisterState(GameState.Spawn, new SpawnCharactersState(this));
        StateMachine.RegisterState(GameState.HeroTurn, new HeroTurnState(this));
        StateMachine.RegisterState(GameState.EnemyState, new EnemyTurnState(this));

        StateMachine.SetState(GameState.Setup);

        PubSub.Instance.RegisterFunction("GoalReached", HeroWin);
    }

    private void Update()
    {
        StateMachine.Update();
    }

    private void HeroWin(object obj)
    {
        Debug.Log("You win");
        Respawn();
    }

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
