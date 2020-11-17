using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Interface : MonoBehaviour
{
    public enum InterfaceType
    {
        TAP_INTERFACE,
        KETTLE_INTERFACE,
        TEAPOT_INTERFACE,
        CUP_INTERFACE,
        SAUCER_INTERFACE,
        ADDITIVE_INTERFACE
    }

    public InterfaceType interfaceType;
}