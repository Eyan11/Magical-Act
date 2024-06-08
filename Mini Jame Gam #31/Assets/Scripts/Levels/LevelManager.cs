using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Transform[] levelArr;
    [SerializeField] private Transform playerHolder;
    private PlayerTransform transformScript;
    private Camera cam;

    [Header ("Settings")]
    [SerializeField] private float curtainCloseTime;

    private int curLevel = 1;

    private void Awake() {
        cam = Camera.main;
        transformScript = playerHolder.GetComponent<PlayerTransform>();
    }

    public void ChangeLevel(int changeInLevel) {

        // clean up current level
        transformScript.ChangeState('N');
        // TODO: start curtain animation

        curLevel += changeInLevel;
        Invoke("SetUpCurrentLevel", curtainCloseTime);

    }

    private void SetUpCurrentLevel() {

        // if beat last level
        if(curLevel > levelArr.Length) {
            // TODO: end the game
            Debug.Log("End Game");
            return;
        }

        Debug.Log("Next Level");

        // move cam to next level
        cam.transform.position = new Vector3(levelArr[curLevel-1].position.x, levelArr[curLevel-1].position.y, cam.transform.position.z);

        // reset positions of magician, rabbit, and hat
        playerHolder.GetChild(0).transform.position = Vector3.zero;
        playerHolder.GetChild(1).transform.position = Vector3.zero;
        playerHolder.GetChild(2).transform.position = Vector3.zero;
        
        // place player in next level
        if(levelArr[curLevel-1].GetChild(0).tag == "Spawn") {
            
            playerHolder.position = levelArr[curLevel-1].GetChild(0).position;
            Debug.Log(levelArr[curLevel-1].GetChild(0).position);
            Debug.Log(playerHolder.position);
        }
        else
            Debug.LogError("Level " + curLevel + "Needs first child to be spawn position and have spawn tag");
        
        // allow movement
        transformScript.ChangeState('M');
    }
}
