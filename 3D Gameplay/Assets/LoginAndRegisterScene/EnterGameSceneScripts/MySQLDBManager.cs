using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MySQLDBManager{

    public static string username;

    public static bool loggedIn { get { return username != null;  } }

    public static void LogOut()
    {
        username = null;
    }
}
