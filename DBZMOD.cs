using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using DBZMOD.Items;

namespace DBZMOD
{
    public class DBZMOD : Mod
    {
        public DBZMOD()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }
        public override void Load()
        {
            StatUI.StatGUIOn = RegisterHotKey("Stat Gui", "N");
            KaiokenKey = RegisterHotKey("Kaioken", "J");
        }
    }
}

