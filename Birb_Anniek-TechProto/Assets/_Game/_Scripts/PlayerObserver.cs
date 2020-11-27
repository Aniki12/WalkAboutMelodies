using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This script manages how a game object should react to the player

public class PlayerObserver : MonoBehaviour
{
    public float cheerDuration = 1f;
    public UnityEvent onPlayMusicEvent;
    public UnityEvent onCheerEvent;
    public UnityEvent onIdleEvent;
    public UnityEvent onCelebrationEvent;
    private RespondToPlayerState respondState;
    private GameMaster gm;
    private Coroutine currentStateCoroutine;
    private int platformCount;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameMaster.GetInstance();
        currentStateCoroutine = StartCoroutine("PlayMusic");
        platformCount = 0;
    }

    // All the states for the FSM with their Unity Events
    IEnumerator PlayMusic() {
        Debug.Log("Start Play Music");
        onPlayMusicEvent.Invoke();

        respondState = RespondToPlayerState.PlayingMusic;
        while (gm.isPlayingAudio)
        {
            Debug.Log("Playing Audio");
            yield return new WaitForFixedUpdate();
        }
        currentStateCoroutine = StartCoroutine("Idle");
        yield return null;
    }   
    
    IEnumerator Cheer() {
        onCheerEvent.Invoke();

        respondState = RespondToPlayerState.Cheering;
        Debug.Log("Start Cheering");
        yield return new WaitForSeconds(cheerDuration);
        currentStateCoroutine = StartCoroutine("Idle");
    }

    IEnumerator Idle() {
        onIdleEvent.Invoke();

        Debug.Log("Start Idle");
        respondState = RespondToPlayerState.IsIdle;
        while (platformCount == gm.playerOrder.Count)
        {
            Debug.Log("Idle");
            platformCount = gm.playerOrder.Count;
            yield return new WaitForFixedUpdate();
        }
        if (gm.playerOrder.Count == gm.platformOrder.Count)
        {
            currentStateCoroutine = StartCoroutine("Celebrate");
        } else if (gm.playerOrder.Count == 0)
        {
            platformCount = 0;
            currentStateCoroutine = StartCoroutine("Idle");
        } else if ((platformCount + 1) == gm.playerOrder.Count)
        {
            platformCount = gm.playerOrder.Count;
            currentStateCoroutine = StartCoroutine("Cheer");
        }
        yield return null;
    }

    // Final state
    IEnumerator Celebrate()
    {
        onCelebrationEvent.Invoke();
        Debug.Log("Celebrating");
        respondState = RespondToPlayerState.Celebration;
        yield break;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

//States for responding to the player
public enum RespondToPlayerState 
   { 
    PlayingMusic, 
    Cheering, 
    IsIdle, 
    Celebration }
