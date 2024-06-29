using System;
using System.Collections.Generic;

namespace SuperBazaar.Models;

public partial class Item
{
    public int Itemid { get; set; }

    public string Itemname { get; set; } = null!;

    public int Itemprice { get; set; }

    public int Itemquantity { get; set; }
}
