using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongData : MonoBehaviour {

    private float BPM = 168;
    private float BPS = 0;
    private string SongTitle = "Pure Ruby";
    private float offset = 0;

    public float timer;
    public GameObject[] hitObject = new GameObject[7];
    public float spawnTime;
    private int hitObjectType;
    private string hitObjectTag;

    private Vector3[] Positions = new Vector3[190];
    private int increment = 0;


    public List<GameObject> spawnedList = new List<GameObject>();
    private int earliestIndex; // The current earliest note
    public bool hasHit;
    private bool startCheck;
    private int sizeOfList;
    private int nextIndex;
    private bool justHit = false;

    public AudioSource audioData;

    // Use this for initialization
        void Start () {

        // BPS
        BPS = (60/BPM);
   
        offset = 0f;

        

        // Positions
        Positions[0] = new Vector3(0, 0, 0);
        Positions[1] = new Vector3(0, 0, 0);
        Positions[2] = new Vector3(0, 0, 0);
        Positions[3] = new Vector3(0, 0, 0);
        Positions[4] = new Vector3(0, 0, 0);
        Positions[5] = new Vector3(0, 0, 0);
        Positions[6] = new Vector3(0, 0, 0);
        Positions[7] = new Vector3(0, 0, 0);
        Positions[8] = new Vector3(0, 0, 0);
        Positions[9] = new Vector3(0, 0, 0);
        Positions[10] = new Vector3(0, 0, 0);
        Positions[11] = new Vector3(0, 0, 0);
        Positions[12] = new Vector3(0, 0, 0);
        Positions[13] = new Vector3(0, 0, 0);
        Positions[14] = new Vector3(0, 0, 0);
        Positions[15] = new Vector3(0, 0, 0);
        Positions[16] = new Vector3(0, 0, 0);
        Positions[17] = new Vector3(0, 0, 0);
        Positions[18] = new Vector3(0, 0, 0);
        Positions[19] = new Vector3(0, 0, 0);
        Positions[20] = new Vector3(0, 0, 0);
        Positions[21] = new Vector3(0, 0, 0);
        Positions[22] = new Vector3(0, 0, 0);
        Positions[23] = new Vector3(0, 0, 0);
        Positions[24] = new Vector3(0, 0, 0);
        Positions[25] = new Vector3(0, 0, 0);
        Positions[26] = new Vector3(0, 0, 0);
        Positions[27] = new Vector3(0, 0, 0);
        Positions[28] = new Vector3(0, 0, 0);
        Positions[29] = new Vector3(0, 0, 0);
        Positions[30] = new Vector3(0, 0, 0);
        Positions[31] = new Vector3(0, 0, 0);
        Positions[32] = new Vector3(0, 0, 0);
        Positions[33] = new Vector3(0, 0, 0);
        Positions[34] = new Vector3(0, 0, 0);
        Positions[35] = new Vector3(0, 0, 0);
        Positions[36] = new Vector3(0, 0, 0);
        Positions[37] = new Vector3(0, 0, 0);
        Positions[38] = new Vector3(0, 0, 0);
        Positions[39] = new Vector3(0, 0, 0);
        Positions[40] = new Vector3(0, 0, 0);
        Positions[41] = new Vector3(0, 0, 0);
        Positions[42] = new Vector3(0, 0, 0);
        Positions[43] = new Vector3(0, 0, 0);
        Positions[44] = new Vector3(0, 0, 0);
        Positions[45] = new Vector3(0, 0, 0);
        Positions[46] = new Vector3(0, 0, 0);
        Positions[47] = new Vector3(0, 0, 0);
        Positions[48] = new Vector3(0, 0, 0);
        Positions[49] = new Vector3(0, 0, 0);
        Positions[50] = new Vector3(0, 0, 0);
        Positions[51] = new Vector3(0, 0, 0);
        Positions[52] = new Vector3(0, 0, 0);
        Positions[53] = new Vector3(0, 0, 0);
        Positions[54] = new Vector3(0, 0, 0);
        Positions[55] = new Vector3(0, 0, 0);
        Positions[56] = new Vector3(0, 0, 0);
        Positions[57] = new Vector3(0, 0, 0);
        Positions[58] = new Vector3(0, 0, 0);
        Positions[59] = new Vector3(0, 0, 0);
        Positions[60] = new Vector3(0, 0, 0);
        Positions[61] = new Vector3(0, 0, 0);
        Positions[62] = new Vector3(0, 0, 0);
        Positions[63] = new Vector3(0, 0, 0);
        Positions[64] = new Vector3(0, 0, 0);
        Positions[65] = new Vector3(0, 0, 0);
        Positions[66] = new Vector3(0, 0, 0);
        Positions[67] = new Vector3(0, 0, 0);
        Positions[68] = new Vector3(0, 0, 0);
        Positions[69] = new Vector3(0, 0, 0);
        Positions[70] = new Vector3(0, 0, 0);
        Positions[71] = new Vector3(0, 0, 0);
        Positions[72] = new Vector3(0, 0, 0);
        Positions[73] = new Vector3(0, 0, 0);
        Positions[74] = new Vector3(0, 0, 0);
        Positions[75] = new Vector3(0, 0, 0);
        Positions[76] = new Vector3(0, 0, 0);
        Positions[77] = new Vector3(0, 0, 0);
        Positions[78] = new Vector3(0, 0, 0);
        Positions[79] = new Vector3(0, 0, 0);
        Positions[80] = new Vector3(0, 0, 0);
        Positions[81] = new Vector3(0, 0, 0);
        Positions[82] = new Vector3(0, 0, 0);
        Positions[83] = new Vector3(0, 0, 0);
        Positions[84] = new Vector3(0, 0, 0);
        Positions[85] = new Vector3(0, 0, 0);
        Positions[86] = new Vector3(0, 0, 0);
        Positions[87] = new Vector3(0, 0, 0);
        Positions[88] = new Vector3(0, 0, 0);
        Positions[89] = new Vector3(0, 0, 0);
        Positions[90] = new Vector3(0, 0, 0);
        Positions[91] = new Vector3(0, 0, 0);
        Positions[92] = new Vector3(0, 0, 0);
        Positions[93] = new Vector3(0, 0, 0);
        Positions[94] = new Vector3(0, 0, 0);
        Positions[95] = new Vector3(0, 0, 0);
        Positions[96] = new Vector3(0, 0, 0);
        Positions[97] = new Vector3(0, 0, 0);
        Positions[98] = new Vector3(0, 0, 0);
        Positions[99] = new Vector3(0, 0, 0);
        Positions[0] = new Vector3(0, 0, 0);
        Positions[1] = new Vector3(0, 0, 0);
        Positions[2] = new Vector3(0, 0, 0);
        Positions[3] = new Vector3(0, 0, 0);
        Positions[4] = new Vector3(0, 0, 0);
        Positions[5] = new Vector3(0, 0, 0);
        Positions[6] = new Vector3(0, 0, 0);
        Positions[7] = new Vector3(0, 0, 0);
        Positions[8] = new Vector3(0, 0, 0);
        Positions[9] = new Vector3(0, 0, 0);
        Positions[100] = new Vector3(0, 0, 0);
        Positions[101] = new Vector3(0, 0, 0);
        Positions[102] = new Vector3(0, 0, 0);
        Positions[103] = new Vector3(0, 0, 0);
        Positions[104] = new Vector3(0, 0, 0);
        Positions[105] = new Vector3(0, 0, 0);
        Positions[106] = new Vector3(0, 0, 0);
        Positions[107] = new Vector3(0, 0, 0);
        Positions[108] = new Vector3(0, 0, 0);
        Positions[109] = new Vector3(0, 0, 0);
        Positions[110] = new Vector3(0, 0, 0);
        Positions[111] = new Vector3(0, 0, 0);
        Positions[112] = new Vector3(0, 0, 0);
        Positions[113] = new Vector3(0, 0, 0);
        Positions[114] = new Vector3(0, 0, 0);
        Positions[115] = new Vector3(0, 0, 0);
        Positions[116] = new Vector3(0, 0, 0);
        Positions[117] = new Vector3(0, 0, 0);
        Positions[118] = new Vector3(0, 0, 0);
        Positions[119] = new Vector3(0, 0, 0);
        Positions[120] = new Vector3(0, 0, 0);
        Positions[121] = new Vector3(0, 0, 0);
        Positions[122] = new Vector3(0, 0, 0);
        Positions[123] = new Vector3(0, 0, 0);
        Positions[124] = new Vector3(0, 0, 0);
        Positions[125] = new Vector3(0, 0, 0);
        Positions[126] = new Vector3(0, 0, 0);
        Positions[127] = new Vector3(0, 0, 0);
        Positions[128] = new Vector3(0, 0, 0);
        Positions[129] = new Vector3(0, 0, 0);
        Positions[130] = new Vector3(0, 0, 0);
        Positions[131] = new Vector3(0, 0, 0);
        Positions[132] = new Vector3(0, 0, 0);
        Positions[133] = new Vector3(0, 0, 0);
        Positions[134] = new Vector3(0, 0, 0);
        Positions[135] = new Vector3(0, 0, 0);
        Positions[136] = new Vector3(0, 0, 0);
        Positions[137] = new Vector3(0, 0, 0);
        Positions[138] = new Vector3(0, 0, 0);
        Positions[139] = new Vector3(0, 0, 0);
        Positions[140] = new Vector3(0, 0, 0);
        Positions[141] = new Vector3(0, 0, 0);
        Positions[142] = new Vector3(0, 0, 0);
        Positions[143] = new Vector3(0, 0, 0);
        Positions[144] = new Vector3(0, 0, 0);
        Positions[145] = new Vector3(0, 0, 0);
        Positions[146] = new Vector3(0, 0, 0);
        Positions[147] = new Vector3(0, 0, 0);
        Positions[148] = new Vector3(0, 0, 0);
        Positions[149] = new Vector3(0, 0, 0);
        Positions[150] = new Vector3(0, 0, 0);
        Positions[151] = new Vector3(0, 0, 0);
        Positions[152] = new Vector3(0, 0, 0);
        Positions[153] = new Vector3(0, 0, 0);
        Positions[154] = new Vector3(0, 0, 0);
        Positions[155] = new Vector3(0, 0, 0);
        Positions[156] = new Vector3(0, 0, 0);
        Positions[157] = new Vector3(0, 0, 0);
        Positions[158] = new Vector3(0, 0, 0);
        Positions[159] = new Vector3(0, 0, 0);
        Positions[160] = new Vector3(0, 0, 0);
        Positions[161] = new Vector3(0, 0, 0);
        Positions[162] = new Vector3(0, 0, 0);
        Positions[163] = new Vector3(0, 0, 0);
        Positions[164] = new Vector3(0, 0, 0);
        Positions[165] = new Vector3(0, 0, 0);
        Positions[166] = new Vector3(0, 0, 0);
        Positions[167] = new Vector3(0, 0, 0);
        Positions[168] = new Vector3(0, 0, 0);
        Positions[169] = new Vector3(0, 0, 0);
        Positions[170] = new Vector3(0, 0, 0);
        Positions[171] = new Vector3(0, 0, 0);
        Positions[172] = new Vector3(0, 0, 0);
        Positions[173] = new Vector3(0, 0, 0);
        Positions[174] = new Vector3(0, 0, 0);
        Positions[175] = new Vector3(0, 0, 0);
        Positions[176] = new Vector3(0, 0, 0);
        Positions[177] = new Vector3(0, 0, 0);
        Positions[178] = new Vector3(0, 0, 0);
        Positions[179] = new Vector3(0, 0, 0);
        Positions[180] = new Vector3(0, 0, 0);
        Positions[181] = new Vector3(0, 0, 0);
        Positions[182] = new Vector3(0, 0, 0);
        Positions[183] = new Vector3(0, 0, 0);
        Positions[184] = new Vector3(0, 0, 0);
        Positions[185] = new Vector3(0, 0, 0);
        Positions[186] = new Vector3(0, 0, 0);
        Positions[187] = new Vector3(0, 0, 0);
        Positions[188] = new Vector3(0, 0, 0);
        Positions[189] = new Vector3(0, 0, 0);

        //spawnTime = BPS;
        spawnTime = BPS * 2;
        hitObjectType = 0;

        earliestIndex = 0;
        hasHit = false;
        startCheck = false;
        sizeOfList = 0;
        nextIndex = 0;

        Invoke("PlayAudio", 0);

    }
	
	// Update is called once per frame
	void Update () {

            timer += Time.deltaTime;

       
        // Spawn a note every quarter note
        if (timer >= spawnTime)
        {
            increment += 1;

            SpawnHitObject(Positions[increment], hitObjectType);
            
            /*
            hitObjectType += 1;

            if (hitObjectType == 7)
            {
                hitObjectType = 0;
            }
           */


            timer = 0;
        }

        // Size of list
        sizeOfList = spawnedList.Count;
        // Next index required to increment for check
        nextIndex = earliestIndex + 2;

        if (startCheck == true)
        {
            // If the index object exists
            if (spawnedList[earliestIndex] != null)
            {
                spawnedList[earliestIndex].GetComponent<TimingAndScore>().isEarliest = true;
            }

            if (spawnedList[earliestIndex] == null && sizeOfList > earliestIndex)
            {
                earliestIndex++;

            }
        }

    }

    public void SpawnHitObject(Vector3 positionPass, int hitObjectTypePass)
    {
        spawnedList.Add(Instantiate(hitObject[hitObjectTypePass], positionPass, Quaternion.Euler(0, 45, 0)));
        startCheck = true;
    }

    void PlayAudio()
    {
        audioData.Play(0);
    }

}
