using UnityEngine;
using TMPro;

public class KeyManager : MonoBehaviour
{
    #region Variables
    public Animator buttonAnimatorD, buttonAnimatorF, buttonAnimatorSpacebar, buttonAnimatorJ, buttonAnimatorK;
    public TextMeshProUGUI totalKeyPressesTextD, totalKeyPressesTextF, totalKeyPressesTextSpacebar, totalKeyPressesTextJ,
        totalKeyPressesTextK;
    private Animator totalKeyPressesDAnimator, totalKeyPressesFAnimator, totalKeyPressesSpacebarAnimator,
        totalKeyPressesJAnimator, totalKeyPressesKAnimator;
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

        // Reference animator
        totalKeyPressesDAnimator = totalKeyPressesTextD.GetComponent<Animator>();
        totalKeyPressesFAnimator = totalKeyPressesTextF.GetComponent<Animator>();
        totalKeyPressesSpacebarAnimator = totalKeyPressesTextSpacebar.GetComponent<Animator>();
        totalKeyPressesJAnimator = totalKeyPressesTextJ.GetComponent<Animator>();
        totalKeyPressesKAnimator = totalKeyPressesTextK.GetComponent<Animator>();
    }

    void Update()
    {
        #region GET KEY DOWN
        // For tracking total presses and playing text animation
        if (Input.GetKeyDown(KeyCode.D))
        {
            totalKeyPressesD++;
            totalKeyPressesTextD.text = totalKeyPressesD.ToString();
            PlayTotalKeyPressTextAnimation(totalKeyPressesDAnimator);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            totalKeyPressesF++;
            totalKeyPressesTextF.text = totalKeyPressesF.ToString();
            PlayTotalKeyPressTextAnimation(totalKeyPressesFAnimator);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            totalKeyPressesSpacebar++;
            totalKeyPressesTextSpacebar.text = totalKeyPressesSpacebar.ToString();
            PlayTotalKeyPressTextAnimation(totalKeyPressesSpacebarAnimator);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            totalKeyPressesJ++;
            totalKeyPressesTextJ.text = totalKeyPressesJ.ToString();
            PlayTotalKeyPressTextAnimation(totalKeyPressesJAnimator);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            totalKeyPressesK++;
            totalKeyPressesTextK.text = totalKeyPressesK.ToString();
            PlayTotalKeyPressTextAnimation(totalKeyPressesKAnimator);
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

        #region KEY UP
        if (Input.GetKeyUp(KeyCode.D))
        {
            PlayKeyReleaseAnimation(KEY_D);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            PlayKeyReleaseAnimation(KEY_F);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            PlayKeyReleaseAnimation(KEY_SPACEBAR);
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            PlayKeyReleaseAnimation(KEY_J);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            PlayKeyReleaseAnimation(KEY_K);
        }
        #endregion
    }

    // Play TotalKeyPressTextAnimation
    public void PlayTotalKeyPressTextAnimation(Animator _animator)
    {
        _animator.Play("EnlargeFade_Animation", 0, 0f);
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

    // Play key release animation
    public void PlayKeyReleaseAnimation(string _key)
    {
        /*
        switch (_key)
        {
            case KEY_D:
                buttonAnimatorD.Play("Key_Release_Animation", 0, 0f);
                break;
            case KEY_F:
                buttonAnimatorF.Play("Key_Release_Animation", 0, 0f);
                break;
            case KEY_SPACEBAR:
                buttonAnimatorSpacebar.Play("Key_Release_Animation", 0, 0f);
                break;
            case KEY_J:
                buttonAnimatorJ.Play("Key_Release_Animation", 0, 0f);
                break;
            case KEY_K:
                buttonAnimatorK.Play("Key_Release_Animation", 0, 0f);
                break;
        }
        */
    }
    #endregion
}
