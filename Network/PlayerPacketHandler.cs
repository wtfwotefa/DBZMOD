﻿using DBZMOD;
using Enums;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Util;

namespace Network
{    
    internal class PlayerPacketHandler : PacketHandler
    {
        // this timer keeps the ki sync from firing *every frame* which really hurts performance.
        public const byte SyncPlayer = 43;
        public const byte RequestSyncAll = 44;
        public const byte SyncTriggers = 45;

        public PlayerPacketHandler(byte handlerType) : base(handlerType)
        {
        }
        
        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (SyncPlayer):
                    ReceiveChangedPlayerData(reader, fromWho);
                    break;
                case (RequestSyncAll):
                    ReceiveSyncAllRequest(reader, fromWho);
                    break;
                case (SyncTriggers):
                    ReceiveSyncTriggers(reader, fromWho);
                    break;
            }
        }

        // should always be to server, from joining player
        public void SendServerSyncAllPlayersRequest(int toWho, int fromWho)
        {
            var packet = GetPacket(RequestSyncAll, fromWho);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTriggerLeft(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            // DebugUtil.Log(string.Format("Sending kiMax2 changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncTriggers, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerLeft);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTriggerRight(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            // DebugUtil.Log(string.Format("Sending kiMax2 changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncTriggers, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerRight);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTriggerUp(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            // DebugUtil.Log(string.Format("Sending kiMax2 changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncTriggers, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerUp);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTriggerDown(int toWho, int fromWho, int whichPlayer, bool isHeld)
        {
            // DebugUtil.Log(string.Format("Sending kiMax2 changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncTriggers, fromWho);
            packet.Write((int)PlayerVarSyncEnum.TriggerDown);
            packet.Write(whichPlayer);
            packet.Write(isHeld);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKiMax2(int toWho, int fromWho, int whichPlayer, int kiMax2)
        {
            // DebugUtil.Log(string.Format("Sending kiMax2 changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.KiMax2);
            packet.Write(whichPlayer);
            packet.Write(kiMax2);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKiMax3(int toWho, int fromWho, int whichPlayer, int kiMax3)
        {
            // DebugUtil.Log(string.Format("Sending kiMax3 changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.KiMax3);
            packet.Write(whichPlayer);
            packet.Write(kiMax3);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKiMaxMult(int toWho, int fromWho, int whichPlayer, float KiMaxMult)
        {
            // DebugUtil.Log(string.Format("Sending kiMaxMult changes from {0} to {1} for player {2} to {3}", fromWho, toWho, whichPlayer, KiMaxMult));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.KiMaxMult);
            packet.Write(whichPlayer);
            packet.Write(KiMaxMult);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsTransforming(int toWho, int fromWho, int whichPlayer, bool IsTransforming)
        {
            // DebugUtil.Log(string.Format("Sending IsTransforming changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsTransforming);
            packet.Write(whichPlayer);
            packet.Write(IsTransforming);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment1(int toWho, int fromWho, int whichPlayer, bool Fragment1)
        {
            // DebugUtil.Log(string.Format("Sending Fragment1 changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment1);
            packet.Write(whichPlayer);
            packet.Write(Fragment1);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment2(int toWho, int fromWho, int whichPlayer, bool Fragment2)
        {
            // DebugUtil.Log(string.Format("Sending Fragment2 changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment2);
            packet.Write(whichPlayer);
            packet.Write(Fragment2);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment3(int toWho, int fromWho, int whichPlayer, bool Fragment3)
        {
            // DebugUtil.Log(string.Format("Sending Fragment3 changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment3);
            packet.Write(whichPlayer);
            packet.Write(Fragment3);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment4(int toWho, int fromWho, int whichPlayer, bool Fragment4)
        {
            // DebugUtil.Log(string.Format("Sending Fragment4 changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment4);
            packet.Write(whichPlayer);
            packet.Write(Fragment4);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedFragment5(int toWho, int fromWho, int whichPlayer, bool Fragment5)
        {
            // DebugUtil.Log(string.Format("Sending Fragment5 changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.Fragment5);
            packet.Write(whichPlayer);
            packet.Write(Fragment5);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsCharging(int toWho, int fromWho, int whichPlayer, bool IsCharging)
        {
            // DebugUtil.Log(string.Format("Sending IsCharging changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsCharging);
            packet.Write(whichPlayer);
            packet.Write(IsCharging);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedJungleMessage(int toWho, int fromWho, int whichPlayer, bool JungleMessage)
        {
            // DebugUtil.Log(string.Format("Sending Jungle Message changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.JungleMessage);
            packet.Write(whichPlayer);
            packet.Write(JungleMessage);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedHellMessage(int toWho, int fromWho, int whichPlayer, bool HellMessage)
        {
            // DebugUtil.Log(string.Format("Sending Hell Message changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.HellMessage);
            packet.Write(whichPlayer);
            packet.Write(HellMessage);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedEvilMessage(int toWho, int fromWho, int whichPlayer, bool EvilMessage)
        {
            // DebugUtil.Log(string.Format("Sending Evil Message changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.EvilMessage);
            packet.Write(whichPlayer);
            packet.Write(EvilMessage);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedMushroomMessage(int toWho, int fromWho, int whichPlayer, bool MushroomMessage)
        {
            // DebugUtil.Log(string.Format("Sending Mushroom Message changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.MushroomMessage);
            packet.Write(whichPlayer);
            packet.Write(MushroomMessage);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsHoldingKiWeapon(int toWho, int fromWho, int whichPlayer, bool IsHoldingKiWeapon)
        {
            // DebugUtil.Log(string.Format("Sending IsHoldingKiWeapon changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsHoldingKiWeapon);
            packet.Write(whichPlayer);
            packet.Write(IsHoldingKiWeapon);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedTraitChecked(int toWho, int fromWho, int whichPlayer, bool traitChecked)
        {
            // DebugUtil.Log(string.Format("Sending traitChecked changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.traitChecked);
            packet.Write(whichPlayer);
            packet.Write(traitChecked);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedPlayerTrait(int toWho, int fromWho, int whichPlayer, string playerTrait)
        {
            // DebugUtil.Log(string.Format("Sending PlayerTrait changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.playerTrait);
            packet.Write(whichPlayer);
            packet.Write(playerTrait);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedIsFlying(int toWho, int fromWho, int whichPlayer, bool IsFlying)
        {
            // DebugUtil.Log(string.Format("Sending IsFlying changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.IsFlying);
            packet.Write(whichPlayer);
            packet.Write(IsFlying);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedChargeMoveSpeed(int toWho, int fromWho, int whichPlayer, float chargeMoveSpeed)
        {            
            var packet = GetPacket(SyncPlayer, fromWho);
            packet.Write((int)PlayerVarSyncEnum.ChargeMoveSpeed);
            packet.Write(whichPlayer);
            packet.Write(chargeMoveSpeed);
            packet.Send(toWho, fromWho);
        }

        public void SendChangedKiCurrent(int toWho, int fromWho, int whichPlayer, int kiCurrent)
        {
            // DebugUtil.Log(string.Format("Sending KiCurrent changes from {0} to {1} for player {2}", fromWho, toWho, whichPlayer));            
            var packet = GetPacket(SyncPlayer, fromWho); ;
            packet.Write((int)PlayerVarSyncEnum.KiCurrent);
            packet.Write(whichPlayer);
            packet.Write(kiCurrent);
            packet.Send(toWho, fromWho);
        }

        // receive a request from a user to forward all other connected user's data.
        public void ReceiveSyncAllRequest(BinaryReader reader, int fromWho)
        {
            // this uses the same dysfunctional loop as the npc checks, because Main.player is nightmare fuel.
            for (int i = 0; i < Main.player.Length; i++)
            {
                // i wish I could tell you why this is a thing.
                Player player = Main.player[i];
                // this. wtf is this terraria. why?
                if (player.whoAmI != i)
                {
                    continue;
                }

                // don't send the players info to themselves.
                if (player.whoAmI == fromWho)
                    continue;

                DebugUtil.Log(string.Format("Sending sync-all packet to {0} for player {1}", player.whoAmI, fromWho));
                MyPlayer modPlayer = Main.player[fromWho].GetModPlayer<MyPlayer>();
                SendChangedKiMax2(player.whoAmI, 256, fromWho, modPlayer.KiMax2);
                SendChangedKiMax3(player.whoAmI, 256, fromWho, modPlayer.KiMax3);
                SendChangedKiMaxMult(player.whoAmI, 256, fromWho, modPlayer.KiMaxMult);
                SendChangedIsTransforming(player.whoAmI, 256, fromWho, modPlayer.IsTransforming);
                SendChangedFragment1(player.whoAmI, 256, fromWho, modPlayer.Fragment1);
                SendChangedFragment2(player.whoAmI, 256, fromWho, modPlayer.Fragment2);
                SendChangedFragment3(player.whoAmI, 256, fromWho, modPlayer.Fragment3);
                SendChangedFragment4(player.whoAmI, 256, fromWho, modPlayer.Fragment4);
                SendChangedFragment5(player.whoAmI, 256, fromWho, modPlayer.Fragment5);
                SendChangedIsCharging(player.whoAmI, 256, fromWho, modPlayer.IsCharging);
                SendChangedJungleMessage(player.whoAmI, 256, fromWho, modPlayer.JungleMessage);
                SendChangedHellMessage(player.whoAmI, 256, fromWho, modPlayer.HellMessage);
                SendChangedEvilMessage(player.whoAmI, 256, fromWho, modPlayer.EvilMessage);
                SendChangedMushroomMessage(player.whoAmI, 256, fromWho, modPlayer.MushroomMessage);
                SendChangedIsHoldingKiWeapon(player.whoAmI, 256, fromWho, modPlayer.IsHoldingKiWeapon);
                SendChangedTraitChecked(player.whoAmI, 256, fromWho, modPlayer.traitChecked);
                SendChangedPlayerTrait(player.whoAmI, 256, fromWho, modPlayer.playerTrait);
                SendChangedIsFlying(player.whoAmI, 256, fromWho, modPlayer.IsFlying);
                SendChangedKiCurrent(player.whoAmI, 256, fromWho, modPlayer.GetKi());
            }
        }

        public void ReceiveSyncTriggers(BinaryReader reader, int fromWho)
        {
            PlayerVarSyncEnum syncEnum = (PlayerVarSyncEnum)reader.ReadInt32();
            int playerNum = reader.ReadInt32();
            MyPlayer player = Main.player[playerNum].GetModPlayer<MyPlayer>();

            // if this is a server, start to assemble the relay packet.
            ModPacket packet = null;
            if (Main.netMode == NetmodeID.Server)
            {
                packet = GetPacket(SyncTriggers, fromWho);
                packet.Write((int)syncEnum);
                packet.Write(playerNum);
            }

            switch(syncEnum)
            {
                case PlayerVarSyncEnum.TriggerLeft:
                    player.IsLeftHeld = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsLeftHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.TriggerRight:
                    player.IsRightHeld = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsRightHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.TriggerUp:
                    player.IsUpHeld = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsUpHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.TriggerDown:
                    player.IsDownHeld = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsDownHeld);
                        packet.Send(-1, fromWho);
                    }
                    break;
            }
        }

        public void ReceiveChangedPlayerData(BinaryReader reader, int fromWho)
        {            
            //Console.WriteLine("Receiving player sync change packet!");
            PlayerVarSyncEnum syncEnum = (PlayerVarSyncEnum)reader.ReadInt32();
            int playerNum = reader.ReadInt32();
            // DebugUtil.Log(string.Format("Receiving changes from {0} for player {1} changing {2}", fromWho, playerNum, syncEnum.ToString()));
            MyPlayer player = Main.player[playerNum].GetModPlayer<MyPlayer>();

            // if this is a server, start to assemble the relay packet.
            ModPacket packet = null;
            if (Main.netMode == NetmodeID.Server)
            {
                packet = GetPacket(SyncPlayer, fromWho);
                packet.Write((int)syncEnum);
                packet.Write(playerNum);
            }
            switch (syncEnum)
            {
                case PlayerVarSyncEnum.KiMax2:
                    player.KiMax2 = reader.ReadInt32();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.KiMax2);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.KiMax3:
                    player.KiMax3 = reader.ReadInt32();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.KiMax3);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.KiMaxMult:
                    player.KiMaxMult = reader.ReadSingle();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.KiMaxMult);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsTransforming:
                    player.IsTransforming = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsTransforming);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment1:
                    player.Fragment1 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.Fragment1);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment2:
                    player.Fragment2 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.Fragment2);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment3:
                    player.Fragment3 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.Fragment3);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment4:
                    player.Fragment4 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.Fragment4);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.Fragment5:
                    player.Fragment5 = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.Fragment5);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsCharging:
                    player.IsCharging = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsCharging);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.JungleMessage:
                    player.JungleMessage = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.JungleMessage);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.HellMessage:
                    player.HellMessage = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.HellMessage);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.EvilMessage:
                    player.EvilMessage = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.EvilMessage);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.MushroomMessage:
                    player.MushroomMessage = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.MushroomMessage);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsHoldingKiWeapon:
                    player.IsHoldingKiWeapon = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsHoldingKiWeapon);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.traitChecked:
                    player.traitChecked = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.traitChecked);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.playerTrait:
                    player.playerTrait = reader.ReadString();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.playerTrait);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.IsFlying:
                    player.IsFlying = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.IsFlying);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.ChargeMoveSpeed:
                    player.chargeMoveSpeed = reader.ReadSingle();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.chargeMoveSpeed);
                        packet.Send(-1, fromWho);
                    }
                    break;
                case PlayerVarSyncEnum.KiCurrent:
                    player.SetKi(reader.ReadInt32());
                    if (Main.netMode == NetmodeID.Server)
                    {
                        packet.Write(player.GetKi());
                        packet.Send(-1, fromWho);
                    }
                    break;
            }
        }
    }
}
