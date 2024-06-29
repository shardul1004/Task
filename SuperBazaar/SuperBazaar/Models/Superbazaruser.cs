using System;
using System.Collections.Generic;

namespace SuperBazaar.Models;

public partial class Superbazaruser
{
    public int Userid { get; set; }

    public long Userphone { get; set; }

    public string Username { get; set; } = null!;
}
