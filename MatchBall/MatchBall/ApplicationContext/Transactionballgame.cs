using System;
using System.Collections.Generic;

namespace MatchBall.ApplicationContext;

public partial class Transactionballgame
{
    public int Userid { get; set; }

    public int TransactionId { get; set; }

    public string Transactiontype { get; set; } = null!;

    public int Amount { get; set; }

    public virtual Usertableballgame User { get; set; } = null!;
}
