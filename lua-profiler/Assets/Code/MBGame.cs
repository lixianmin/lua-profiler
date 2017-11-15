
/********************************************************************
created:    2017-11-15
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

using System;
using UnityEngine;
using LuaInterface;

namespace Client
{
    public class MBGame : LuaClient
    {
        public static IntPtr GetLuaStatePtr ()
        {
            return Instance._GetLuaStatePtr ();
        }
    }
}