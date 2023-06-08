using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Version : MonoBehaviour
{
    private void Awake(){
        GetComponent<TextMeshProUGUI>().text = Application.version;
    }
}
