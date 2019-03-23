using Microsoft.Xna.Framework;
using System;
using DBZMOD.Extensions;
using DBZMOD.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace DBZMOD.NPCs.Bosses
{
    //Thanks a bit to examplemod's flutterslime for helping with organization
	public class FriezaShip : ModNPC
	{
        private Vector2 hoverDistance = new Vector2(130, 180);
        private float hoverCooldown = 500;
        private int slamTimer = 0;
        private int slamCoolDownTimer = 0;
        private bool locationSelected = false;

        private int YHoverTimer = 0;
        private int XHoverTimer = 0;

        const int AIStageSlot = 0;
        const int AITimerSlot = 1;

        public float AIStage
        {
            get { return npc.ai[AIStageSlot]; }
            set { npc.ai[AIStageSlot] = value; }
        }

        public float AITimer
        {
            get { return npc.ai[AITimerSlot]; }
            set { npc.ai[AITimerSlot] = value; }
        }

        const int Stage_Hover = 0;
        const int Stage_Slam = 1;
        const int Stage_Barrage = 2;
        const int Stage_Homing = 3;
        const int Stage_Saiba = 4;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("A Frieza Force Ship");
			Main.npcFrameCount[npc.type] = 8;
		}

        public override void SetDefaults()
        {
            npc.width = 110;
            npc.height = 60;
            npc.damage = 26;
            npc.defense = 10;
            npc.lifeMax = 3600;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = Item.buyPrice(0, 3, 25, 80);
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/TheUnexpectedArrival");
        }

        //To-Do: Add the rest of the stages to the AI. Make green saibaman code.
        //Make the speed of the ship's movements increase with less health, less time between stages as well?
        //Boss loot: Drops Undecided material that's used to create a guardian class armor set (frieza cyborg set). Alternates drops between a weapon and accessory, accessory is arm cannon mk2, weapon is a frieza force beam rifle. Expert item is the mechanical amplifier.
        //Spawn condition: Near the ocean you can find a frieza henchmen, if he runs away then you'll get an indicator saying the ship will be coming the next morning.


        public override void AI()
        {
            Player player = Main.player[npc.target];
            npc.TargetClosest(true);
            
            //Runaway if no players are alive
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, 10f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }

            //Make sure the stages loop back around
            if(AIStage == 5)
            {
                AIStage = Stage_Hover;
            }

            //Speed between stages drastically increased with health lost
            if(npc.life < npc.lifeMax * 0.70f)
            {
                hoverCooldown = 400;
                if (npc.life < npc.lifeMax * 0.40f)
                {
                    hoverCooldown = 300;
                }
                if (npc.life < npc.lifeMax * 0.15f)
                {
                    hoverCooldown = 100;
                }
            }

            
            //General movement (stage 0)
            if (AIStage == Stage_Hover)
            {
                //Y Hovering
                if (Vector2.Distance(new Vector2(0, player.position.Y), new Vector2(0, npc.position.Y)) != hoverDistance.Y)
                {

                    if (Vector2.Distance(new Vector2(0, player.position.Y), new Vector2(0, npc.position.Y)) > hoverDistance.Y)
                    {
                        //float hoverSpeedY = (2f + Main.rand.NextFloat(3, 8));
                        //Add a little bit of delay before moving, this lets melee players possibly get a hit in
                        YHoverTimer++;
                        if(YHoverTimer > 15)
                        {
                            npc.velocity.Y = 2f;
                        }
                    }
                    else if (Vector2.Distance(new Vector2(0, player.position.Y), new Vector2(0, npc.position.Y)) < hoverDistance.Y)
                    {
                        //float hoverSpeedY = (-2f + Main.rand.NextFloat(-3, -8));
                        YHoverTimer++;
                        if (YHoverTimer > 15)
                        {
                            npc.velocity.Y = -2f;
                        }
                    }
                }
                else
                {
                    npc.velocity.Y = 0;
                    YHoverTimer = 0;
                }
                //X Hovering, To-Do: Make the ship not just center itself on the player, have some left and right alternating movement?
                if (Vector2.Distance(new Vector2(0, player.position.X), new Vector2(0, npc.position.X)) != hoverDistance.X)
                {
                    //float hoverSpeedY = (-2f + Main.rand.NextFloat(-3, -8));
                    XHoverTimer++;
                    if (XHoverTimer > 30)
                    {
                        npc.velocity.X = (2.5f * npc.direction);
                        if (AITimer > 400)
                        {
                            npc.velocity.X = (5f * npc.direction);
                        }
                        
                    }
                }
                else
                {
                    npc.velocity.X = 0;
                    XHoverTimer = 0;
                }
                
                //Next Stage
                AITimer++;
                if (AITimer > hoverCooldown)
                {
                    StageAdvance();
                    AITimer = 0;
                }

            }




            //Slam attack (stage 1) - Quickly moves to directly above the player, then waits a second before slamming straight down.
            //To-Do: Make slam do increased contact damage. Fix bug where the ship flies down into the ground. Fix dust on tile collide not appearing. Fix afterimage on slam not working.
            
            if (AIStage == Stage_Slam)
            {
                slamTimer++;
                if (slamTimer > 20)
                {
                    npc.velocity.X = 0;
                    locationSelected = true;
                    AITimer++;
                    if (AITimer > 20)
                    {
                        if (AITimer == 21)
                        {
                            npc.noTileCollide = false;
                            npc.velocity.Y = 18f;
                        }
                        if (CoordinateExtensions.IsPositionInTile(npc.position))
                        {
                            npc.velocity.Y = 0;
                            ExplodeEffect();
                            SoundHelper.PlayCustomSound("Sounds/Kiplosion", npc.position, 1.0f);
                        }

                        if (npc.velocity.Y == 0)
                        {
                            npc.velocity.Y = -1f;        
                        }

                        if (npc.velocity.Y == -1f)
                        {
                            slamCoolDownTimer++;
                        }
                        if (slamCoolDownTimer > 20)
                        {
                            StageAdvance();
                            AITimer = 0;
                            slamCoolDownTimer = 0;
                            slamTimer = 0;
                            locationSelected = false;
                            npc.noTileCollide = true;
                        }
                    }
                }
            }
            //Vertical projectile barrage (stage 2) - Fires a barrage of projectiles upwards that randomly spread out and fall downwards which explode on ground contact

            if (AIStage == Stage_Barrage)
            {
                AITimer++;
                npc.velocity.Y = 0;
                npc.velocity.X = 0;

                if (AITimer == 10)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);

                    if (npc.life < npc.lifeMax * 0.70f) //Fire 4 extra projectiles if below 70% health
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -4f, mod.ProjectileType("FFBarrageBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    }
                }

                if (AITimer > 60)
                {
                    if (npc.life < npc.lifeMax * 0.60f)
                    {
                        StageAdvance();
                    }
                    else
                    {
                        AIStage = Stage_Hover;
                    }
                    AITimer = 0;
                }
            }

            //Vertical projectile barrage + homing (stage 3) - Fires 2 projectiles in opposite arcs diagonally from the ship, after 3 seconds they stop, after 1 second both will fly towards the player.
            // These projectiles are stronger than the barrage ones, but also slower.

            if (AIStage == Stage_Homing)
            {
                npc.velocity.Y = 0;
                npc.velocity.X = 0;

                if (AITimer == 0)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2.5f, -1f, mod.ProjectileType("FFHomingBlast"), npc.damage / 3, 3f, Main.myPlayer);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -2.5f, -1f, mod.ProjectileType("FFHomingBlast"), npc.damage / 3, 3f, Main.myPlayer);

                    if (npc.life < npc.lifeMax * 0.50f) //Fire an extra projectile upwards if below 50% health
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -1f, mod.ProjectileType("FFHomingBlast"), npc.damage / 4, 3f, Main.myPlayer);
                    }
                }
                AITimer++;
                if (AITimer > 60)
                {
                    if (npc.life < npc.lifeMax * 0.40f)
                    {
                        StageAdvance();
                    }
                    else
                    {
                        AIStage = Stage_Hover;
                    }
                    AITimer = 0;
                }
            }

            //To-Do: Summon saibamen (stage 4) - Summons a green saiba from the ship, green dust when this happens to make it look smoother (Perhaps make this something after 40% HP)
            if (Main.netMode != 1 && AIStage == Stage_Saiba)
            {
                if (AITimer == 0)
                {
                    int saiba = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("SaibaGreen"));
                    Main.npc[saiba].netUpdate = true;
                    npc.netUpdate = true;
                }
                AITimer++;
                if (AITimer > 60)
                {
                    StageAdvance();
                    AITimer = 0;
                }
            }

            //Main.NewText(AIStage);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
            if (AIStage == Stage_Slam)
            {
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
                for (int k = 0; k < npc.oldPos.Length; k++)
                {
                    Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(lightColor) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, null, color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
                }
            }
			return true;
		}

        private void StageAdvance()
        {
            //if Below 30% health, randomly pick a stage
            if(npc.life < npc.lifeMax * 0.30f)
            {
                int NextStage = Main.rand.Next(0, 4);

                AIStage = NextStage;

            }
            //otherwise, go to next stage
            else
            {
                AIStage++;
            }

        }

        //Animations
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if(AIStage == Stage_Barrage || AIStage == Stage_Homing)
            {
                npc.frameCounter += 3;
            }
            else
            {
                npc.frameCounter++;
            }
            if (npc.frameCounter > 4)
            {
                frame++;
                npc.frameCounter = 0;
            }
            if(frame > 7) //Make it 7 because 0 is counted as a frame, making it 8 frames
            {
                frame = 0;
            }

            npc.frame.Y = frameHeight * frame;
        }

        /*public override void NPCLoot()
        {
            if (Main.rand.Next(20) == 0)
            {
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MajinNucleus"));
                }
            }
        }*/

        public void ExplodeEffect()
        {
            for (int num619 = 0; num619 < 3; num619++)
            {
                float scaleFactor9 = 3f;
                if (num619 == 1)
                {
                    scaleFactor9 = 3f;
                }
                int num620 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num620].velocity *= scaleFactor9;
                Gore gore97 = Main.gore[num620];
                gore97.velocity.X = gore97.velocity.X + 1f;
                Gore gore98 = Main.gore[num620];
                gore98.velocity.Y = gore98.velocity.Y + 1f;
                num620 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num620].velocity *= scaleFactor9;
                Gore gore99 = Main.gore[num620];
                gore99.velocity.X = gore99.velocity.X - 1f;
                Gore gore100 = Main.gore[num620];
                gore100.velocity.Y = gore100.velocity.Y + 1f;
                num620 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num620].velocity *= scaleFactor9;
                Gore gore101 = Main.gore[num620];
                gore101.velocity.X = gore101.velocity.X + 1f;
                Gore gore102 = Main.gore[num620];
                gore102.velocity.Y = gore102.velocity.Y - 1f;
                num620 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num620].velocity *= scaleFactor9;
                Gore gore103 = Main.gore[num620];
                gore103.velocity.X = gore103.velocity.X - 1f;
                Gore gore104 = Main.gore[num620];
                gore104.velocity.Y = gore104.velocity.Y - 1f;
            }
        }

    }
}
