﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POCKETPAL_API.Models
{
    public class ModelMessage
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
    public class ModelRespond
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}