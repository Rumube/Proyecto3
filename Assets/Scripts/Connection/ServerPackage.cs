using System.Collections;
using System.Collections.Generic;

namespace ServerPack
{
    /// <summary>Sent from server to client.</summary>
    public enum ServerPackets
    {
        nul = 0,
        IdTablet = 1,
        StudentSelection = 2,
        StartGame = 3,
        GameDifficulty = 4,
        UpdateTeamPoints = 5,
        PauseGame = 6,
        Quit = 7,
        Disconnect = 8

    }

    /// <summary>Sent from client to server.</summary>
    public enum ClientPackets
    {
        nul = 0,
        studentsEndCall = 1,
        selectedStudentGame = 2,
        matchData = 3,
        viewFinalScore = 4
    }

    public class ServerPackage
    {
        public string _toUser;
        public string _fromUser;
        public ClientPackets _typePackageClient;
        public ServerPackets _typePackageServer;
        public struct TabletInfo
        {
            public int _idTablet;
        }
        public TabletInfo _tabletInfo;
        public struct StudentsInfo
        {
            public Tablet _studentsToTablets;
        }
        public StudentsInfo _studentsInfo;
        public struct CallingDone
        {
            public bool _isDone;
        }
        public CallingDone _callingDone;
        public struct MinigameTime
        {
            public int _minutes;
            public int _seconds;
        }
        public MinigameTime _minigameTime;
        public struct SelectStudentGame
        {
            public string _studentName;
            public string _gameName;
        }
        public SelectStudentGame _selectStudentGame;

        public struct PauseGame
        {
            public bool _pause;
        }
        public PauseGame _pauseGame;
        public struct GameDifficulty
        {
            public int _level;
        }
        public GameDifficulty _gameDifficulty;
        public struct MatchData
        {
            public string _studentName;
            public string _gameName;
            public int _team;
            public int _averageSuccess;
            public int _averageErrors;
            public float _averageGameTime;
            public int _averagePoints;
            public int _gameLevel;
        }
        public MatchData _matchData;
        public struct RankingPoints
        {
            public Dictionary<int, int> _teamPoints;
        }
        public RankingPoints _rankingPoints;
        /// <summary>Creates a new empty packet (without an ID).</summary>
        public ServerPackage()
        {

        }
    }
}
