﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInfo : MonoBehaviour
{
    [SerializeField] RotationByFinger swiperPannel;
    public string CubeTheme;
    public string CubeColor;

    public void ChangeCube() {
        GameObject playerCubePosition = GameObject.FindGameObjectWithTag("PlayerCubePosition");

        Destroy(playerCubePosition.transform.Find("Cube(Clone)").gameObject);

        PlayerPrefs.SetString("CubeTheme", CubeTheme);
        PlayerPrefs.SetString("CubeColor", CubeColor);

        GameObject cube = Instantiate(Resources.Load<GameObject>(CubeTheme + "/" + CubeColor + "/Cube"));
        cube.transform.position = playerCubePosition.transform.position;
        cube.transform.rotation = playerCubePosition.transform.rotation;
        cube.transform.SetParent(playerCubePosition.transform);

        swiperPannel.ChangeCube(cube);
    }
}
