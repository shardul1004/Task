using System;
using System.Collections.Generic;

namespace MatchBall.ApplicationContext;

public partial class Usertableballgame
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Hpassword { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public virtual ICollection<Transactionballgame> Transactionballgames { get; set; } = new List<Transactionballgame>();
}
