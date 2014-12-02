using UnityEngine;

public class Configuration :MonoBehaviour {
    public bool genGroundDecal = false;

    public bool USE_BUNDLE = false;

    public static int MAX_DOOR_NUMBER = 10;

    public string bundleURL = Application.streamingAssetsPath+"/";

    public string tcpHost = "127.0.0.1";
    public int tcpPort = 1234;
    public string udpHost = "127.0.0.1";
    public int udpPort = 5678;



}
