using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MidiJack;
using System;

[RequireComponent(typeof(Dropdown))]
public class SetDropdownFromPorts : MonoBehaviour {

    List<string> allDevices;
    Dropdown dropdown;

    void Start () {
        RefreshList();
        TryOpenSavedDevice();
	}

    private void TryOpenSavedDevice()
    {
        if (m_deviceToOpen.Length > 0)
        {
            int indexToOpen = allDevices.IndexOf(m_deviceToOpen);
            if (indexToOpen > -1)
            {
                SwitchToDevice(indexToOpen);
            }
        }
    }

    private void OnEnable()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(SwitchToDevice);
    }

    private void OnDisable()
    {
        dropdown.onValueChanged.RemoveListener(SwitchToDevice);
    }

    void SwitchToDevice(int index)
    {
        dropdown.value = index;
        MidiDriver.CloseDevices();
        MidiDriver.OpenDevice((uint)index);
        m_deviceToOpen = allDevices[index];
    }

    string m_deviceToOpen
    {
        get { return PlayerPrefs.GetString("deviceToOpen", ""); }
        set { PlayerPrefs.SetString("deviceToOpen", value); }
    }

    List<string> GetDeviceNames()
    {
        var deviceNames = new List<string>();
        var endpointCount = MidiDriver.CountEndpoints();
        for (uint i = 0; i < endpointCount; i++)
        {
            string name = MidiDriver.GetEndpointName(i);
            deviceNames.Add(name);
        }
        return deviceNames;
    }

    public void RefreshList()
    {
        dropdown.ClearOptions();
        allDevices = GetDeviceNames();
        dropdown.AddOptions(allDevices);
    }
}
