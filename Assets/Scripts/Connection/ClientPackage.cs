using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClientPack
{
    /// <summary>Sent from server to client.</summary>
    public enum ServerPackets
    {
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
        studentsEndCall = 1,
        selectedStudentGame = 2,
        matchData = 3,
    }

    public class ClientPackage
    {
        public int _toUser;
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
        public ClientPackage()
        {

        }
        /// <summary>Creates a new packet with a given ID. Used for sending.</summary>
        /// <param name="_id">The packet ID.</param>
        public ClientPackage(int idTypePackage)
        {

        }
        /// <summary>Creates a packet from which data can be read. Used for receiving.</summary>
        /// <param name="_data">The bytes to add to the packet.</param>
        public ClientPackage(string data)
        {

        }
    }
}
