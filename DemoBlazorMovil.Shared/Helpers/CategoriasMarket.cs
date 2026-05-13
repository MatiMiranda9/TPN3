using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazorMovil.Shared.Helpers
{
    public static class CategoriasMarket
    {
        public const string Snacks = "Snacks";
        public const string Bebidas = "Bebidas";
        public const string Combos = "Combos";

        public static List<string> Todas => new()
    {
        Snacks,
        Bebidas,
        Combos
    };
    }
}
