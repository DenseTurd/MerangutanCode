using UnityEngine;
using TMPro;

public class Hud : MonoBehaviour
{
    public TMP_Text coinsTxt;
    public GameObject loadingSplash;

    void Start()
    {
        UpdateCoinsVal();    
    }

    public void UpdateCoinsVal()
    {
        coinsTxt.text = DTPrefs.GetInt(DTPrefs.GetString(Strs.playerID) + Strs.DTCoins).ToString();
    }

    public void HideLoadingSplash()
    {
        loadingSplash.SetActive(false);
    }
}
