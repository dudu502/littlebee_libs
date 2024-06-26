﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Common.Log
{
    public interface ILogger
    {
        bool IsDebugEnabled { get; set; }
        void Info(string message);
        void Warn(string message);
        void Error(string message);
    }
}
