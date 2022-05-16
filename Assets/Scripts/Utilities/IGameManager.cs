using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    public enum GAME_STATE_CLIENT
    {
        init = 0,
        searching = 1,
        selectStudent = 2,
        playing = 3,
        pause = 4,
        gameOver = 5,
        ranking = 6,
        globalRanking = 7,
    }

    public void PreviousConfigurationState();
    public void ConnectionState();
    public void RandomizeStudentsList();
    public void Shuffle<T>(IList<T> list);
    public void SelectStudentAndGame();
    public GAME_STATE_CLIENT GetClientState();
    public void SetClientState(GAME_STATE_CLIENT gameStateClient);
    public void SetReturnToCommonScene(bool value);
}
