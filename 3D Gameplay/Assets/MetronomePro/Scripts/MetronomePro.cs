// Created by Carlos Arturo Rodriguez Silva (Legend)

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class MetronomePro : MonoBehaviour {
	
	[Header("Variables")]
	public bool active = false;

	[Space(5)]

	public AudioSource metronomeAudioSource;
	public AudioClip highClip;
	public AudioClip lowClip;

	[Space(5)]
	public Text txtBPM;
	public Text txtOffsetMS;
	public InputField BPMInputField;
	public InputField OffsetInputField;
	public Text txtState;

	[Space(5)]

	public AudioSource songAudioSource;

	[Header("Metronome (this values will be overrided by Song Data)")]

	public double Bpm = 140.0f;
	public double OffsetMS = 0;

	public int Step = 4;
	public int Base = 4;

	public int CurrentMeasure = 0;
	public int CurrentStep = 0;
	public int CurrentTick;

	public List<Double> songTickTimes;

	double interval;

	public bool neverPlayed = true;



    // NEW
    public GameObject[] hitObject = new GameObject[7];
    public float spawnTime;
    private int hitObjectType;
    private string hitObjectTag;

    private Vector3[] Positions = new Vector3[401];
    private int increment = 0;


    public List<GameObject> spawnedList = new List<GameObject>();
    private int earliestIndex; // The current earliest note
    public bool hasHit;
    private bool startCheck;
    private int sizeOfList;
    private int nextIndex;
    private bool justHit = false;

    public AudioSource audioData;

    float timer = 0f;

    void Start () {

        // Positions
        Positions[0] = new Vector3(0, 0, 0);
        Positions[1] = new Vector3(-300, 0, 90);
        Positions[2] = new Vector3(-250, 0, 90);
        Positions[3] = new Vector3(300, 0, -90);
        Positions[4] = new Vector3(250, 0, -90);
        Positions[5] = new Vector3(-300, 0, -90);
        Positions[6] = new Vector3(-250, 0, -90);
        Positions[7] = new Vector3(300, 0, 90);
        Positions[8] = new Vector3(250, 0, 90);
        Positions[9] = new Vector3(-300, 0, 90);
        Positions[10] = new Vector3(-250, 0, 90);
        Positions[11] = new Vector3(300, 0, -90);
        Positions[12] = new Vector3(250, 0, -90);
        Positions[13] = new Vector3(-300, 0, -90);
        Positions[14] = new Vector3(-250, 0, -90);
        Positions[15] = new Vector3(300, 0, 90);
        Positions[16] = new Vector3(250, 0, 90);
        Positions[17] = new Vector3(0, 0, 0);
        Positions[18] = new Vector3(50, 0, 0);
        Positions[19] = new Vector3(-50, 0, 0);
        Positions[20] = new Vector3(100, 0, 0);
        Positions[21] = new Vector3(-100, 0, 0);
        Positions[22] = new Vector3(150, 0, 0);
        Positions[23] = new Vector3(-150, 0, 0);
        Positions[24] = new Vector3(-200, 0, 0);
        Positions[25] = new Vector3(250, 0, 0);
        Positions[26] = new Vector3(-250, 0, 0);
        Positions[27] = new Vector3(300, 0, 0);
        Positions[28] = new Vector3(-300, 0, 0);
        Positions[29] = new Vector3(250, 0, 0);
        Positions[30] = new Vector3(-250, 0, 0);
        Positions[31] = new Vector3(200, 0, 0);
        Positions[32] = new Vector3(-200, 0, 0);
        Positions[33] = new Vector3(150, 0, 0);
        Positions[34] = new Vector3(-150, 0, 0);
        Positions[35] = new Vector3(100, 0, 0);
        Positions[36] = new Vector3(-100, 0, 0);
        Positions[37] = new Vector3(50, 0, 0);
        Positions[38] = new Vector3(-50, 0, 0);
        Positions[39] = new Vector3(0, 0, 0);
        Positions[40] = new Vector3(0, 0, -50);
        Positions[41] = new Vector3(0, 0, 50);
        Positions[42] = new Vector3(0, 0, -100);
        Positions[43] = new Vector3(0, 0, 100);
        Positions[44] = new Vector3(0, 0, -50);
        Positions[45] = new Vector3(0, 0, 50);
        Positions[46] = new Vector3(0, 0, 0);
        Positions[47] = new Vector3(50, 0, 50);
        Positions[48] = new Vector3(50, 0, -50);
        Positions[49] = new Vector3(100, 0, 50);
        Positions[50] = new Vector3(100, 0, -50);
        Positions[51] = new Vector3(150, 0, 100);
        Positions[52] = new Vector3(150, 0, -100);
        Positions[53] = new Vector3(100, 0, 50);
        Positions[54] = new Vector3(100, 0, -50);
        Positions[55] = new Vector3(50, 0, 50);
        Positions[56] = new Vector3(50, 0, -50);
        Positions[57] = new Vector3(0, 0, 0);
        Positions[58] = new Vector3(-50, 0, 50);
        Positions[59] = new Vector3(-50, 0, -50);
        Positions[60] = new Vector3(-100, 0, 50);
        Positions[61] = new Vector3(-100, 0, -50);
        Positions[62] = new Vector3(-150, 0, 50);
        Positions[63] = new Vector3(-150, 0, -50);
        Positions[64] = new Vector3(-200, 0, 50);
        Positions[65] = new Vector3(-200, 0, -50);
        Positions[66] = new Vector3(-250, 0, 50);
        Positions[67] = new Vector3(-250, 0, -50);
        Positions[68] = new Vector3(-300, 0, 50);
        Positions[69] = new Vector3(300, 0, -100);
        Positions[70] = new Vector3(250, 0, -100);
        Positions[71] = new Vector3(200, 0, -100);
        Positions[72] = new Vector3(150, 0, -100);
        Positions[73] = new Vector3(100, 0, -100);
        Positions[74] = new Vector3(50, 0, -100);
        Positions[75] = new Vector3(0, 0, 0);
        Positions[76] = new Vector3(50, 0, 100);
        Positions[77] = new Vector3(100, 0, 100);
        Positions[78] = new Vector3(150, 0, 100);
        Positions[79] = new Vector3(200, 0, 100);
        Positions[80] = new Vector3(250, 0, 100);
        Positions[81] = new Vector3(300, 0, 100);
        Positions[82] = new Vector3(0, 0, 0);
        Positions[83] = new Vector3(300, 0, 0);
        Positions[84] = new Vector3(250, 0, 0);
        Positions[85] = new Vector3(200, 0, 0);
        Positions[86] = new Vector3(150, 0, 0);
        Positions[87] = new Vector3(100, 0, 0);
        Positions[88] = new Vector3(50, 0, 0);
        Positions[89] = new Vector3(0, 0, 0);
        Positions[90] = new Vector3(50, 0, 0);
        Positions[91] = new Vector3(100, 0, 0);
        Positions[92] = new Vector3(150, 0, 0);
        Positions[93] = new Vector3(200, 0, 0);
        Positions[94] = new Vector3(250, 0, 0);
        Positions[95] = new Vector3(300, 0, 0);
        Positions[96] = new Vector3(250, 0, 0);
        Positions[97] = new Vector3(200, 0, 0);
        Positions[98] = new Vector3(250, 0, 0);
        Positions[99] = new Vector3(300, 0, 0);
        Positions[100] = new Vector3(250, 0, 50);
        Positions[101] = new Vector3(200, 0, 50);
        Positions[102] = new Vector3(250, 0, 50);
        Positions[103] = new Vector3(300, 0, 50);
        Positions[104] = new Vector3(250, 0, 100);
        Positions[105] = new Vector3(200, 0, 100);
        Positions[106] = new Vector3(250, 0, 100);
        Positions[107] = new Vector3(300, 0, 100);
        Positions[108] = new Vector3(250, 0, 50);
        Positions[109] = new Vector3(200, 0, 50);
        Positions[110] = new Vector3(250, 0, 50);
        Positions[111] = new Vector3(300, 0, 50);
        Positions[112] = new Vector3(0, 0, 0);
        Positions[113] = new Vector3(0, 0, 50);
        Positions[114] = new Vector3(0, 0, 100);
        Positions[115] = new Vector3(0, 0, 0);
        Positions[116] = new Vector3(0, 0, 50);
        Positions[117] = new Vector3(0, 0, 100);
        Positions[118] = new Vector3(0, 0, 50);
        Positions[119] = new Vector3(0, 0, 0);
        Positions[120] = new Vector3(0, 0, 50);
        Positions[121] = new Vector3(0, 0, 100);
        Positions[122] = new Vector3(0, 0, 50);
        Positions[123] = new Vector3(0, 0, 0);
        Positions[124] = new Vector3(0, 0, -50);
        Positions[125] = new Vector3(0, 0, -100);
        Positions[126] = new Vector3(0, 0, -50);
        Positions[127] = new Vector3(0, 0, 0);
        Positions[128] = new Vector3(0, 0, -50);
        Positions[129] = new Vector3(0, 0, -100);
        Positions[130] = new Vector3(0, 0, -50);
        Positions[131] = new Vector3(0, 0, 0);
        Positions[132] = new Vector3(0, 0, -50);
        Positions[133] = new Vector3(0, 0, -100);
        Positions[134] = new Vector3(0, 0, -50);
        Positions[135] = new Vector3(0, 0, 0);
        Positions[136] = new Vector3(-300, 0, 90);
        Positions[137] = new Vector3(-250, 0, 90);
        Positions[138] = new Vector3(300, 0, -90);
        Positions[139] = new Vector3(250, 0, -90);
        Positions[140] = new Vector3(-300, 0, -90);
        Positions[140] = new Vector3(-250, 0, -90);
        Positions[141] = new Vector3(300, 0, 90);
        Positions[142] = new Vector3(250, 0, 90);
        Positions[143] = new Vector3(-300, 0, 90);
        Positions[144] = new Vector3(-250, 0, 90);
        Positions[145] = new Vector3(300, 0, -90);
        Positions[146] = new Vector3(250, 0, -90);
        Positions[147] = new Vector3(-300, 0, -90);
        Positions[148] = new Vector3(-250, 0, -90);
        Positions[149] = new Vector3(300, 0, 90);
        Positions[150] = new Vector3(250, 0, 90);
        Positions[151] = new Vector3(0, 0, 0);
        Positions[152] = new Vector3(50, 0, 0);
        Positions[153] = new Vector3(-50, 0, 0);
        Positions[154] = new Vector3(100, 0, 0);
        Positions[155] = new Vector3(-100, 0, 0);
        Positions[156] = new Vector3(150, 0, 0);
        Positions[157] = new Vector3(-150, 0, 0);
        Positions[158] = new Vector3(-200, 0, 0);
        Positions[159] = new Vector3(250, 0, 0);
        Positions[160] = new Vector3(-250, 0, 0);
        Positions[161] = new Vector3(300, 0, 0);
        Positions[162] = new Vector3(-300, 0, 0);
        Positions[163] = new Vector3(250, 0, 0);
        Positions[164] = new Vector3(-250, 0, 0);
        Positions[165] = new Vector3(200, 0, 0);
        Positions[166] = new Vector3(-200, 0, 0);
        Positions[167] = new Vector3(150, 0, 0);
        Positions[168] = new Vector3(-150, 0, 0);
        Positions[169] = new Vector3(100, 0, 0);
        Positions[170] = new Vector3(-100, 0, 0);
        Positions[171] = new Vector3(50, 0, 0);
        Positions[172] = new Vector3(-50, 0, 0);
        Positions[173] = new Vector3(0, 0, 0);
        Positions[174] = new Vector3(0, 0, -50);
        Positions[175] = new Vector3(0, 0, 50);
        Positions[176] = new Vector3(0, 0, -100);
        Positions[177] = new Vector3(0, 0, 100);
        Positions[178] = new Vector3(0, 0, -50);
        Positions[179] = new Vector3(0, 0, 50);
        Positions[180] = new Vector3(0, 0, 0);
        Positions[181] = new Vector3(50, 0, 50);
        Positions[182] = new Vector3(50, 0, -50);
        Positions[183] = new Vector3(100, 0, 50);
        Positions[184] = new Vector3(100, 0, -50);
        Positions[185] = new Vector3(150, 0, 100);
        Positions[186] = new Vector3(150, 0, -100);
        Positions[187] = new Vector3(100, 0, 50);
        Positions[188] = new Vector3(100, 0, -50);
        Positions[189] = new Vector3(50, 0, 50);
        Positions[190] = new Vector3(50, 0, -50);
        Positions[191] = new Vector3(0, 0, 0);
        Positions[192] = new Vector3(-50, 0, 50);
        Positions[193] = new Vector3(-50, 0, -50);
        Positions[194] = new Vector3(-100, 0, 50);
        Positions[195] = new Vector3(-100, 0, -50);
        Positions[196] = new Vector3(-150, 0, 50);
        Positions[197] = new Vector3(-150, 0, -50);
        Positions[198] = new Vector3(-200, 0, 50);
        Positions[199] = new Vector3(-200, 0, -50);
        Positions[200] = new Vector3(-250, 0, 50);
        Positions[201] = new Vector3(-250, 0, -50);
        Positions[202] = new Vector3(-300, 0, 50);
        Positions[203] = new Vector3(300, 0, -100);
        Positions[204] = new Vector3(250, 0, -100);
        Positions[205] = new Vector3(200, 0, -100);
        Positions[206] = new Vector3(150, 0, -100);
        Positions[207] = new Vector3(100, 0, -100);
        Positions[208] = new Vector3(50, 0, -100);
        Positions[209] = new Vector3(0, 0, 0);
        Positions[210] = new Vector3(50, 0, 100);
        Positions[211] = new Vector3(100, 0, 100);
        Positions[212] = new Vector3(150, 0, 100);
        Positions[213] = new Vector3(200, 0, 100);
        Positions[214] = new Vector3(250, 0, 100);
        Positions[215] = new Vector3(300, 0, 100);
        Positions[216] = new Vector3(0, 0, 0);
        Positions[217] = new Vector3(300, 0, 0);
        Positions[218] = new Vector3(250, 0, 0);
        Positions[219] = new Vector3(200, 0, 0);
        Positions[220] = new Vector3(150, 0, 0);
        Positions[221] = new Vector3(100, 0, 0);
        Positions[222] = new Vector3(50, 0, 0);
        Positions[223] = new Vector3(0, 0, 0);
        Positions[224] = new Vector3(50, 0, 0);
        Positions[225] = new Vector3(100, 0, 0);
        Positions[226] = new Vector3(150, 0, 0);
        Positions[227] = new Vector3(200, 0, 0);
        Positions[228] = new Vector3(250, 0, 0);
        Positions[229] = new Vector3(300, 0, 0);
        Positions[230] = new Vector3(250, 0, 0);
        Positions[231] = new Vector3(200, 0, 0);
        Positions[232] = new Vector3(250, 0, 0);
        Positions[233] = new Vector3(300, 0, 0);
        Positions[234] = new Vector3(250, 0, 50);
        Positions[235] = new Vector3(200, 0, 50);
        Positions[236] = new Vector3(250, 0, 50);
        Positions[237] = new Vector3(300, 0, 50);
        Positions[238] = new Vector3(250, 0, 100);
        Positions[239] = new Vector3(200, 0, 100);
        Positions[240] = new Vector3(250, 0, 100);
        Positions[241] = new Vector3(300, 0, 100);
        Positions[242] = new Vector3(250, 0, 50);
        Positions[243] = new Vector3(200, 0, 50);
        Positions[244] = new Vector3(250, 0, 50);
        Positions[245] = new Vector3(300, 0, 50);
        Positions[246] = new Vector3(0, 0, 0);
        Positions[247] = new Vector3(0, 0, 50);
        Positions[248] = new Vector3(0, 0, 100);
        Positions[249] = new Vector3(0, 0, 0);
        Positions[250] = new Vector3(0, 0, 50);
        Positions[251] = new Vector3(0, 0, 100);
        Positions[252] = new Vector3(0, 0, 50);
        Positions[253] = new Vector3(0, 0, 0);
        Positions[254] = new Vector3(0, 0, 50);
        Positions[255] = new Vector3(0, 0, 100);
        Positions[256] = new Vector3(0, 0, 50);
        Positions[257] = new Vector3(0, 0, 0);
        Positions[258] = new Vector3(0, 0, -50);
        Positions[259] = new Vector3(0, 0, -100);
        Positions[260] = new Vector3(0, 0, -50);
        Positions[261] = new Vector3(0, 0, 0);
        Positions[262] = new Vector3(0, 0, -50);
        Positions[263] = new Vector3(0, 0, -100);
        Positions[264] = new Vector3(0, 0, -50);
        Positions[265] = new Vector3(0, 0, 0);
        Positions[266] = new Vector3(0, 0, -50);
        Positions[267] = new Vector3(0, 0, -100);
        Positions[268] = new Vector3(0, 0, -50); 
        Positions[269] = new Vector3(-300, 0, 90);
        Positions[270] = new Vector3(-250, 0, 90);
        Positions[271] = new Vector3(300, 0, -90);
        Positions[272] = new Vector3(250, 0, -90);
        Positions[273] = new Vector3(-300, 0, -90);
        Positions[274] = new Vector3(-250, 0, -90);
        Positions[275] = new Vector3(300, 0, 90);
        Positions[276] = new Vector3(250, 0, 90);
        Positions[277] = new Vector3(-300, 0, 90);
        Positions[278] = new Vector3(-250, 0, 90);
        Positions[279] = new Vector3(300, 0, -90);
        Positions[280] = new Vector3(250, 0, -90);
        Positions[281] = new Vector3(-300, 0, -90);
        Positions[282] = new Vector3(-250, 0, -90);
        Positions[283] = new Vector3(300, 0, 90);
        Positions[284] = new Vector3(250, 0, 90);
        Positions[285] = new Vector3(0, 0, 0);
        Positions[286] = new Vector3(50, 0, 0);
        Positions[287] = new Vector3(-50, 0, 0);
        Positions[288] = new Vector3(100, 0, 0);
        Positions[289] = new Vector3(-100, 0, 0);
        Positions[290] = new Vector3(150, 0, 0);
        Positions[291] = new Vector3(-150, 0, 0);
        Positions[292] = new Vector3(-200, 0, 0);
        Positions[293] = new Vector3(250, 0, 0);
        Positions[294] = new Vector3(-250, 0, 0);
        Positions[295] = new Vector3(300, 0, 0);
        Positions[296] = new Vector3(-300, 0, 0);
        Positions[297] = new Vector3(250, 0, 0);
        Positions[298] = new Vector3(-250, 0, 0);
        Positions[299] = new Vector3(200, 0, 0);
        Positions[300] = new Vector3(-200, 0, 0);
        Positions[301] = new Vector3(150, 0, 0);
        Positions[302] = new Vector3(-150, 0, 0);
        Positions[303] = new Vector3(100, 0, 0);
        Positions[304] = new Vector3(-100, 0, 0);
        Positions[305] = new Vector3(50, 0, 0);
        Positions[306] = new Vector3(-50, 0, 0);
        Positions[307] = new Vector3(0, 0, 0);
        Positions[308] = new Vector3(0, 0, -50);
        Positions[309] = new Vector3(0, 0, 50);
        Positions[310] = new Vector3(0, 0, -100);
        Positions[311] = new Vector3(0, 0, 100);
        Positions[312] = new Vector3(0, 0, -50);
        Positions[313] = new Vector3(0, 0, 50);
        Positions[314] = new Vector3(0, 0, 0);
        Positions[315] = new Vector3(50, 0, 50);
        Positions[316] = new Vector3(50, 0, -50);
        Positions[317] = new Vector3(100, 0, 50);
        Positions[318] = new Vector3(100, 0, -50);
        Positions[319] = new Vector3(150, 0, 100);
        Positions[320] = new Vector3(150, 0, -100);
        Positions[321] = new Vector3(100, 0, 50);
        Positions[322] = new Vector3(100, 0, -50);
        Positions[323] = new Vector3(50, 0, 50);
        Positions[324] = new Vector3(50, 0, -50);
        Positions[325] = new Vector3(0, 0, 0);
        Positions[326] = new Vector3(-50, 0, 50);
        Positions[327] = new Vector3(-50, 0, -50);
        Positions[328] = new Vector3(-100, 0, 50);
        Positions[329] = new Vector3(-100, 0, -50);
        Positions[330] = new Vector3(-150, 0, 50);
        Positions[331] = new Vector3(-150, 0, -50);
        Positions[332] = new Vector3(-200, 0, 50);
        Positions[333] = new Vector3(-200, 0, -50);
        Positions[334] = new Vector3(-250, 0, 50);
        Positions[335] = new Vector3(-250, 0, -50);
        Positions[336] = new Vector3(-300, 0, 50);
        Positions[337] = new Vector3(300, 0, -100);
        Positions[338] = new Vector3(250, 0, -100);
        Positions[339] = new Vector3(200, 0, -100);
        Positions[340] = new Vector3(150, 0, -100);
        Positions[341] = new Vector3(100, 0, -100);
        Positions[342] = new Vector3(50, 0, -100);
        Positions[343] = new Vector3(0, 0, 0);
        Positions[344] = new Vector3(50, 0, 100);
        Positions[345] = new Vector3(100, 0, 100);
        Positions[346] = new Vector3(150, 0, 100);
        Positions[347] = new Vector3(200, 0, 100);
        Positions[348] = new Vector3(250, 0, 100);
        Positions[349] = new Vector3(300, 0, 100);
        Positions[350] = new Vector3(0, 0, 0);
        Positions[351] = new Vector3(300, 0, 0);
        Positions[352] = new Vector3(250, 0, 0);
        Positions[353] = new Vector3(200, 0, 0);
        Positions[354] = new Vector3(150, 0, 0);
        Positions[355] = new Vector3(100, 0, 0);
        Positions[356] = new Vector3(50, 0, 0);
        Positions[357] = new Vector3(0, 0, 0);
        Positions[358] = new Vector3(50, 0, 0);
        Positions[359] = new Vector3(100, 0, 0);
        Positions[360] = new Vector3(150, 0, 0);
        Positions[361] = new Vector3(-50, 0, 0);
        Positions[362] = new Vector3(0, 0, 0);
        Positions[363] = new Vector3(0, 0, -50);
        Positions[364] = new Vector3(0, 0, 50);
        Positions[365] = new Vector3(0, 0, -100);
        Positions[366] = new Vector3(0, 0, 100);
        Positions[367] = new Vector3(0, 0, -50);
        Positions[368] = new Vector3(0, 0, 50);
        Positions[369] = new Vector3(0, 0, 0);
        Positions[370] = new Vector3(50, 0, 50);
        Positions[371] = new Vector3(50, 0, -50);
        Positions[372] = new Vector3(100, 0, 50);
        Positions[373] = new Vector3(100, 0, -50);
        Positions[374] = new Vector3(150, 0, 100);
        Positions[375] = new Vector3(150, 0, -100);
        Positions[376] = new Vector3(100, 0, 50);
        Positions[378] = new Vector3(100, 0, -50);
        Positions[379] = new Vector3(50, 0, 50);
        Positions[380] = new Vector3(50, 0, -50);
        Positions[381] = new Vector3(0, 0, 0);
        Positions[382] = new Vector3(-50, 0, 50);
        Positions[383] = new Vector3(-50, 0, -50);
        Positions[384] = new Vector3(-100, 0, 50);
        Positions[385] = new Vector3(-100, 0, -50);
        Positions[386] = new Vector3(-150, 0, 50);
        Positions[387] = new Vector3(-150, 0, -50);
        Positions[388] = new Vector3(-200, 0, 50);
        Positions[389] = new Vector3(-200, 0, -50);
        Positions[390] = new Vector3(-250, 0, 50);
        Positions[391] = new Vector3(-250, 0, -50);
        Positions[392] = new Vector3(-300, 0, 50);
        Positions[393] = new Vector3(300, 0, -100);
        Positions[394] = new Vector3(250, 0, -100);
        Positions[395] = new Vector3(200, 0, -100);
        Positions[396] = new Vector3(150, 0, -100);
        Positions[397] = new Vector3(100, 0, -100);
        Positions[398] = new Vector3(50, 0, -100);
        Positions[399] = new Vector3(0, 0, 0);
        Positions[400] = new Vector3(50, 0, 100);


        hitObjectType = 0;

        earliestIndex = 0;
        hasHit = false;
        startCheck = false;
        sizeOfList = 0;
        nextIndex = 0;

    }

	public void GetSongData (double _bpm, double _offsetMS, int _base, int _step) {
		Bpm = _bpm;
		OffsetMS = _offsetMS;
		Base = _base;
		Step = _step;
	}

	// Set the new BPM when is playing
	public void UpdateBPM () {
		try {
		var newBPMFloat = float.Parse (BPMInputField.text);
		Bpm = (double)newBPMFloat;

		txtBPM.text = "BPM: " + Bpm.ToString("F");
		txtState.text = "";

		SetDelay ();
		} catch {
			txtState.text = "Please enter the new BPM value correctly.";
			Debug.Log ("Please enter the new BPM value correctly.");
		}
	}

    public void Update()
    {
        // Size of list
        sizeOfList = spawnedList.Count;
        // Next index required to increment for check
        nextIndex = earliestIndex + 2;

        if (startCheck == true)
        {
            // If the index object exists
            if (spawnedList[earliestIndex] != null)
            {
                //spawnedList[earliestIndex].GetComponent<TimingAndScore>().canBeHit = true;
            }

            if (spawnedList[earliestIndex] == null && sizeOfList > earliestIndex)
            {
                earliestIndex++;
            }
        }


    }

	// Set the new Offset when is playing
	public void UpdateOffset () {
		try {
		var newOffsetFloat = int.Parse (OffsetInputField.text);
		OffsetMS = newOffsetFloat;

		txtOffsetMS.text = "Offset: " + OffsetMS.ToString () + " MS";
		txtState.text = "";

		SetDelay ();
		} catch {
			txtState.text = "Please enter the new Offset value correctly.";
			Debug.Log ("Please enter the new Offset value correctly.");
		}
	}


	void SetDelay () {
		bool isPlaying = false;

		if (songAudioSource.isPlaying) {
			isPlaying = true;
		}


		songAudioSource.Pause ();

		CalculateIntervals ();
		CalculateActualStep ();

		if (isPlaying) {
			songAudioSource.Play ();
		}
	}

	// Play Metronome
	public void Play () {
		if (neverPlayed) {
			CalculateIntervals ();
		}

		neverPlayed = false;
		active = true;
    }

	// Pause Metronome
	public void Pause () {
		active = false;
	}

	// Stop Metronome
	public void Stop () {
		active = false;

		CurrentMeasure = 0;
		CurrentStep = 4;
		CurrentTick = 0;
	}

	// Calculate Time Intervals for the song
	public void CalculateIntervals () {
		try {

        active = false;
		var multiplier = Base / Step;
		var tmpInterval = 60f / Bpm;
		interval = tmpInterval / multiplier;

		int i = 0;

		songTickTimes.Clear ();

			while (interval * i <= songAudioSource.clip.length) {
				songTickTimes.Add ((interval * i) + (OffsetMS / 1000f));
				i++;
			}

			active = true;
		} catch {
			txtState.text = "There isn't an Audio Clip assigned in the Player.";
			Debug.LogWarning ("There isn't an Audio Clip assigned in the Player.");
		}
	}

	// Calculate Actual Step when the user changes song position in the UI
	public void CalculateActualStep () {
		active = false;

		// Get the Actual Step searching the closest Song Tick Time using the Actual Song Time
		for (int i = 0; i < songTickTimes.Count; i++) {
			if (songAudioSource.time < songTickTimes[i]) {
				CurrentMeasure = (i / Base);
				CurrentStep = (int)((((float)i / (float)Base) - (i / Base)) * 4);
				if (CurrentStep == 0) {
					CurrentMeasure = 0;
					CurrentStep = 4;
				} else {
					CurrentMeasure++;
				}

				CurrentTick = i;
				Debug.Log ("Metronome Synchronized at Tick: " + i + " Time: "+ songTickTimes[i]);
				break;
			}
		}
		active = true;
	}

	// Read Audio (this function executes from Unity Audio Thread)
	void OnAudioFilterRead (float[] data, int channels) {
		if (!active)

           
        return;
       
        // You can't execute any function of Unity here because this function is working on Unity Audio Thread (this ensure the Metronome Accuracy)
        // To Fix that you need to execute your function on Main Thread again, don't worry i created an easy way to do that :D
        // There are so much other fixes to do this, like Ninja Thread.
        ToMainThread.AssignNewAction ().ExecuteOnMainThread (CalculateTicks());
	}
		
	// Metronome Main function, this calculates the times to make a Tick, Step Count, Metronome Sounds, etc.
	IEnumerator CalculateTicks () {
		if (!active)
			yield return null;

		// Check if the song time is greater than the current tick Time
		if (songAudioSource.time >= songTickTimes [CurrentTick]) {

			CurrentTick++;

			if (CurrentTick >= songTickTimes.Count) {
				active = false;
			}

			// If the Current Step is greater than the Step, reset it and increment the Measure
			if (CurrentStep >= Step) {
				CurrentStep = 1;
				CurrentMeasure++;
				metronomeAudioSource.clip = highClip;
			} else {
				CurrentStep++;
				metronomeAudioSource.clip = lowClip;
			}

			// Call OnTick functions
			StartCoroutine (OnTick ());
		}

		yield return null;
	}

	// Tick Time (execute here all what you want)
	IEnumerator OnTick () {

        // YOUR FUNCTION HERE


        
        increment += 1;

        hitObjectType += 1;
        if (hitObjectType == 7)
        {
            hitObjectType = 0;
        }
        SpawnHitObject(Positions[increment], hitObjectType);
        



		Debug.Log ("Current Step: " + CurrentStep + "/" + Step);
		yield return null;
	}

    public void SpawnHitObject(Vector3 positionPass, int hitObjectTypePass)
    {
        spawnedList.Add(Instantiate(hitObject[hitObjectTypePass], positionPass, Quaternion.Euler(0, 45, 0)));
        startCheck = true;
    }
}