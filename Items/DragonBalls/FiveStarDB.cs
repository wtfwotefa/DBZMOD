using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.DragonBalls
{
    public class FiveStarDB : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("5 Star Dragon Ball");
            Tooltip.SetDefault("A mystical ball with 5 stars inscribed on it.");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = 0;
            item.rare = -12;
        }
    }
}