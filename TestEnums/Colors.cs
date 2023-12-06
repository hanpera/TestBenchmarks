using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEscapades.EnumGenerators;

namespace TestEnums
{
    [EnumExtensions]
    internal enum Colors
    {
        Red,
        Green,
        [Display(Name = "Giallo")]
        Blue
    }
}
