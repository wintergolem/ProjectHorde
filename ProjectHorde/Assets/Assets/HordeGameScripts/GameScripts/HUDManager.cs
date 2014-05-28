using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {
    enum TextState { normal , whiteOut , clearOut , whiteIn , clearIn , clear };
    TextState waveTextState;
	GameManagerScript gameManager;
	PlayerInventoryScript playerInventory;
    GunClass activeGun;

	public GUIText weaponName;
	public GUIText weaponAmmo;
	public GUIText instruction;
    public GUIText waveCount;
    public GUIText score;
    public GUIText fired;
    public GUIText reloading;

    //private base variables
    float fTimeTextTransition = 0;

    //public base variables
    public bool bBetweenWave = true;

    public float fTimeToWhiteOut = 1;
    public float fTimeToClearOut = 0.5f;
    public float fTimeToWhiteIn = 2;
    public float fTimeToClearIn = 1f;
	// Use this for initialization
	void Start () 
	{
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManagerScript>();
		playerInventory = gameManager.player.GetComponent<PlayerInventoryScript> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
        //get active gun
		activeGun = playerInventory.gunInventory [playerInventory.iActiveIndex];
        //display active gun
		weaponAmmo.text = activeGun.AmmoToString ();
		weaponName.text = activeGun.sName;
       //wave count
        waveCount.text = gameManager.iCurrentWave.ToString();
        //wave count text color
        UpdateWaveTextColor();
        //score
        score.text = "Score: " + gameManager.iPlayerScore;

        //due to lack of art

        //reloading
        if (playerInventory.gunInventory[playerInventory.iActiveIndex].bReloading)
            reloading.enabled = true;
        else
            reloading.enabled = false;
        //fire rate display
        if (playerInventory.gunInventory[playerInventory.iActiveIndex].bFired)
        {
            fired.enabled = true;
        }
        else
            fired.enabled = false;
            

	}

    void UpdateWaveTextColor()
    {
        if (gameManager.bBetweenWaves != bBetweenWave)
        {
            if (!bBetweenWave)
            {
                //wave ended
                if (waveTextState == TextState.normal) //check to make sure text hasn't already started its process
                    waveTextState = TextState.whiteOut;
            }
            else
            {
                //wave start
                if (waveTextState == TextState.clear)//check to make sure text hasn't already started its process
                    waveTextState = TextState.clearIn;
            }
            bBetweenWave = gameManager.bBetweenWaves;
        }

        fTimeTextTransition += Time.deltaTime;
        switch( waveTextState )
        {
            case TextState.normal:
                break;
            case TextState.whiteOut:
                waveCount.color = Color.Lerp(waveCount.color, Color.white, fTimeTextTransition / fTimeToWhiteOut);
                if (fTimeTextTransition >= fTimeToWhiteOut)
                {
                    waveTextState = TextState.clearOut;
                    fTimeTextTransition = 0;
                }
                break;
            case TextState.clearOut:
                waveCount.color = Color.Lerp(waveCount.color, Color.clear, fTimeTextTransition / fTimeToClearOut);
                if (fTimeTextTransition >= fTimeToClearOut)
                {
                    waveTextState = TextState.clear;
                    fTimeTextTransition = 0;
                }
                break;
            case TextState.clear:
                break;
            case TextState.clearIn:
                waveCount.color = Color.Lerp(waveCount.color, Color.white, fTimeTextTransition / fTimeToClearIn);
                if (fTimeTextTransition >= fTimeToClearIn)
                {
                    waveTextState = TextState.whiteIn;
                    fTimeTextTransition = 0;
                }
                break;
            case TextState.whiteIn:
                waveCount.color = Color.Lerp(waveCount.color, Color.red, fTimeTextTransition / fTimeToWhiteIn);
                if (fTimeTextTransition >= fTimeToWhiteIn)
                {
                    waveTextState = TextState.normal;
                    fTimeTextTransition = 0;
                }
                break;
            
            default:
                break;
        }
    }

	public void DisplayInstruction( string a_instruction)
	{
		instruction.text = a_instruction;
	}

	public void EndDisplay ()
	{
		instruction.text = "";
	}

	public bool CompareInstruction(string a_string)
	{
		if (a_string == instruction.text)
			return true;
		return false;
	}
}
