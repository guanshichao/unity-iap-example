using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour {
    private const string bundleID = "com.PDGAMES.test1";
    private const string product1 = bundleID + ".cost1";
    private const string product2 = bundleID + ".nocost1";
    private void Update()
    {
        if (StoreKit.ConsumeProduct(product1))
        {
            Debug.LogFormat("Consume:{0}",product1);
        }
        else if (StoreKit.ConsumeProduct(product2))
        {
            Debug.LogFormat("Consume:{0}", product2);
        }
    }
    private void OnGUI()
    {
        GUILayout.Label("StoreKit.isAvailable:"+ StoreKit.isAvailable);
        GUILayout.Label("StoreKit.isProcessing:" + StoreKit.isProcessing);
        if (Button("Install:"+bundleID))
        {
            StoreKit.Install(bundleID);
        }
        if (Button("Buy:" + product1))
        {
            StoreKit.Buy(product1);
        }
        if (Button("Buy:" + product2))
        {
            StoreKit.Buy(product2);
        }
    }
    bool Button(string p)
    {
        return GUILayout.Button(p, GUILayout.Width(100), GUILayout.Height(100));
    }
}
