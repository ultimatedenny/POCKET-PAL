﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POCKETPAL_API.Models
{
    public class ModelMenuList
    {
        public string Menu_Name { get; set; }
        public string IsActive { get; set; }
    }
    public class ModelMenuAccess
    {
        public string MenuName { get; set; }
        public bool IsAllow { get; set; }
    }
}