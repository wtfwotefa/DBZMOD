﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Consumables.KaioFragments
{
    public class KaioFragment3 : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 48;
            item.height = 48;
            item.consumable = true;
            item.maxStack = 1;
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.value = 0;
            item.rare = 6;
            item.potion = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kaioken 10x");
            Tooltip.SetDefault("Unlocks the next level of kaioken.");
        }


        public override bool UseItem(Player player)
        {
            MyPlayer.ModPlayer(player).kaioFragment3 = true;
            return true;

        }
        public override bool CanUseItem(Player player)
        {
            if (MyPlayer.ModPlayer(player).kaioFragment3 || !MyPlayer.ModPlayer(player).kaioFragment2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}