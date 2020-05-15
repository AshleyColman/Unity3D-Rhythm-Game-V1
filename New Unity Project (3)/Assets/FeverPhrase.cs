public class FeverPhrase
{
    #region Variables
    // Int
    private int phraseIndex; // Phrase index
    private int nextObjectID; // Next object id to be hit and checked
    private int[] phraseObjectIDArr; // All hit object ID's for all notes in the fever phrase

    // Bool
    private bool phraseBroken; // Whether the phrase has been broken by a missed note or is still active
    private bool phraseStarted; // Has the fever phrase started
    private bool phraseFinished; // Has the fever phrase finished
    #endregion

    #region Properties
    public int PhraseIndex
    {
        set { phraseIndex = value; }
        get { return phraseIndex; }
    }

    public int NextObjectID
    {
        set { nextObjectID = value; }
        get { return nextObjectID; }
    }

    public int[] PhraseObjectIDArr
    {
        set { phraseObjectIDArr = value; }
        get { return phraseObjectIDArr; }
    }

    public bool PhraseBroken
    {
        set { phraseBroken = value; }
        get { return phraseBroken; }
    }

    public bool PhraseStarted
    {
        set { phraseStarted = value; }
        get { return phraseStarted; }
    }

    public bool PhraseFinished
    {
        set { phraseFinished = value; }
        get { return phraseFinished; }
    }
    #endregion

    // TEST FUNCTIONS
    public void Contructor(int _totalNotesInPhrase, int _phraseIndex)
    {
        phraseIndex = _phraseIndex;
        phraseBroken = false;
        phraseStarted = false;
        PhraseFinished = false;

        phraseObjectIDArr = new int[_totalNotesInPhrase];

        int num = 0;

        switch (_phraseIndex)
        {
            case 0:
                for (int i = 0; i < phraseObjectIDArr.Length; i++)
                {
                    phraseObjectIDArr[i] = i;
                }
                break;
            case 1:
                num = 19;
                for (int i = 0; i < phraseObjectIDArr.Length; i++)
                {
                    phraseObjectIDArr[i] = num;
                    num++;
                }
                break;
            case 2:
                num = 39;
                for (int i = 0; i < phraseObjectIDArr.Length; i++)
                {
                    phraseObjectIDArr[i] = num;
                    num++;
                }
                break;
        }

    }
}
