﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class TypeOfContract
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<DocumentTemplate> DocumentTemplates { get; set; }
    }
}
