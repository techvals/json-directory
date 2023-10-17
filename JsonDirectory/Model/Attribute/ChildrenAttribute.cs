﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonDirectoryNetCore.Model.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ChildrenAttribute : System.Attribute
    {
        public string Name
        {
            get { return "Children"; }
        }
    }
}
