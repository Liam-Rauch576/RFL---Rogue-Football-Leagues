using UnityEngine;
using System.Collections.Generic;

public class PossessionManager : MonoBehaviour
{
    public static PossessionManager instance { get; private set; }

    private Player currentPossessed;
    public List<Player> offense;

    private void Awake()
    {
        instance = this;
    }

    public void GivePossession(Player newPlayer)
    {
        if(currentPossessed != null) 
        {
            currentPossessed.SetPossession(false);
        }

        currentPossessed = newPlayer;
        currentPossessed.SetPossession(true);
    }
}
