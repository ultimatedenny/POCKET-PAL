﻿using System;
using System.Collections.Generic;
using System.Text;

namespace POCKETPAL.Classes
{
    public interface INotification
    {
        void CreateNotification(String title, String message);
    }
}
