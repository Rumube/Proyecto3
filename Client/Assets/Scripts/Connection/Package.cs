using System.Collections;
using System.Collections.Generic;

/// <summary>Sent from server to client.</summary>
public enum ServerPackets
{
    IdTablet = 1,
    SelectStudent = 2,
    UnselectStudent = 3,
    StartGame = 4,
    GameDifficulty = 5,
    UpdateTeamPoints = 6,
    PauseGame = 7,
    Quit = 8,
    Disconnect = 9

}

/// <summary>Sent from client to server.</summary>
public enum ClientPackets
{
    connect = 1,
    selectedStudentGame = 2,
    matchData = 3,
}

public class Package
{
    public struct Info
    {
        public int _idTablet;
        public int _idStudent;
        public string _nameStudent;
    }
    public Info _info;

    public struct Data
    {
        public int _idPackage;
        public int _idSender;
        public string _jsonData;
    }
    public Data _data;
    /// <summary>Creates a new empty packet (without an ID).</summary>
    public Package()
    {

    }
    /// <summary>Creates a new packet with a given ID. Used for sending.</summary>
    /// <param name="_id">The packet ID.</param>
    public Package(int idTypePackage)
    {

    }
    /// <summary>Creates a packet from which data can be read. Used for receiving.</summary>
    /// <param name="_data">The bytes to add to the packet.</param>
    public Package(string data)
    {

    }
}
