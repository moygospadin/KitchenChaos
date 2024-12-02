using System;
using UnityEngine;

public class GameManager : MonoBehaviour {


    public static GameManager Instance {
        get; private set;
    }

    public event EventHandler OnStateChanged;
    private enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State state;
    private float waitingToStartTimer = 1f;
    private float countdownToToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 10f;
    private void Awake() {
        state = State.WaitingToStart;
        Instance = this;
    }


    private void Update() {
        switch (state) {

            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f) {

                    state = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.CountdownToStart:
                countdownToToStartTimer -= Time.deltaTime;
                if (countdownToToStartTimer < 0f) {

                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f) {

                    state = State.GameOver;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:

                break;



        }

    }


    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsCountDownToStartActive() {
        return state == State.CountdownToStart;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }


    public float GetCountdownToStartTime() {
        return countdownToToStartTimer;
    }


    public float GetGamePlayingTimerNormalized() {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);

    }
}
