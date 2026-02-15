using System;
using System.Collections;
using System.Collections.Generic;
using Towers.std;
using UnityEngine;

public static class DPSGlobalManager
{
    public static int KillsByDPSTowers { get; private set; }
    public static int TotalKilledByAllTowers { get; private set; }
    public static int CurrentUpgrade { get; private set; }

    public static event Action<int> OnKillsChanged;

    public static void RegisterKill(VariousMech source)
    {
        TotalKilledByAllTowers++;

        if (source is VariousTowerMechanicsDPSTower)
        {
            KillsByDPSTowers++;
            if (KillsByDPSTowers % 10 == 0)
            {
                CurrentUpgrade++;
            }
            OnKillsChanged?.Invoke(CurrentUpgrade);
        }
    }
}


