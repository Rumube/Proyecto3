using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Tablet { 
    //Id from WebSocket Session
    public int _id;
    public List<Student> _students;
    public string _currentStudent;
    public string _currentGame;
    public int _score;
}
