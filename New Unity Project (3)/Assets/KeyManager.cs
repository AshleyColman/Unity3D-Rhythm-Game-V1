using UnityEngine;
using TMPro;

public class KeyManager : MonoBehaviour
{
    #region Variables
    public Animator buttonAnimatorD, buttonAnimatorF, buttonAnimatorSpacebar, buttonAnimatorJ, buttonAnimatorK;
    public int totalKeyPressesD, totalKeyPressesF, totalKeyPressesSpacebar, totalKeyPressesJ, totalKeyPressesK;
    private const string KEY_D = "D", KEY_F = "F", KEY_SPACEBAR = "SPACEBAR", KEY_J = "J", KEY_K = "K";
    #endregion

    #region Function
    private void Start()
    {
        // Initialize
        totalKeyPressesD = 0;
        totalKeyPressesF = 0;
        totalKeyPressesSpacebar = 0;
        totalKeyPressesJ = 0;
        totalKeyPressesK = 0;
    }

    void Update()
    {
        #region GET KEY DOWN
        // For tracking total presses and playing text animation
        if (Input.GetKeyDown(KeyCode.D))
        {
            totalKeyPressesD++;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            totalKeyPressesF++;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            totalKeyPressesSpacebar++;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            totalKeyPressesJ++;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            totalKeyPressesK++;
        }
        #endregion

        #region GET KEY
        if (Input.GetKey(KeyCode.D))
        {
            PlayKeyHeldAnimation(KEY_D);
        }
        if (Input.GetKey(KeyCode.F))
        {
            PlayKeyHeldAnimation(KEY_F);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            PlayKeyHeldAnimation(KEY_SPACEBAR);
        }
        if (Input.GetKey(KeyCode.J))
        {
            PlayKeyHeldAnimation(KEY_J);
        }
        if (Input.GetKey(KeyCode.K))
        {
            PlayKeyHeldAnimation(KEY_K);
        }
        #endregion
    }

    // Play key held animation
    public void PlayKeyHeldAnimation(string _key)
    {
        switch (_key)
        {
            case KEY_D:
                buttonAnimatorD.Play("Key_Held_Animation", 0, 0f);
                break;
            case KEY_F:
                buttonAnimatorF.Play("Key_Held_Animation", 0, 0f);
                break;
            case KEY_SPACEBAR:
                buttonAnimatorSpacebar.Play("Key_Held_Animation", 0, 0f);
                break;
            case KEY_J:
                buttonAnimatorJ.Play("Key_Held_Animation", 0, 0f);
                break;
            case KEY_K:
                buttonAnimatorK.Play("Key_Held_Animation", 0, 0f);
                break;
        }
    }
    #endregion
}
