using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using static Terraria.GameContent.Bestiary.IL_BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;

namespace Willy.Content.NPCs
{
    [AutoloadHead]
    public class Willy : ModNPC
    {
        public override string Texture => "Willy/Content/NPCs/Willy";
        public const string FishShopName = "Fish Shop";
        public const string AccessoryShopName = "Bait & Accessory Shop";
        public const string QuestFishShop = "Quest Fish Shop";
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25;

            NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 90;
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };
            NPC.Happiness
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Love)
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
                .SetBiomeAffection<HallowBiome>(AffectionLevel.Hate)
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate)
                .SetNPCAffection(NPCID.Angler, AffectionLevel.Love)
                ;

        }
        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 30;
            NPC.defense = 25;
            NPC.lifeMax = 300;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Guide;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Willy got teleported here from another world and uses this opportunity to catch interesting fish"),
                new FlavorTextBestiaryInfoElement("Mods.Willy.Bestiary.Willy")
            });
        }
        public override string GetChat()
        {
            switch (Main.rand.Next(10))
            {
                case 0:
                    return "Ahoy there. Looks like decent weather for fishing, eh?";
                case 1:
                    return "Have you had much luck in the local waters? You look like you could be a strong angler if you set your mind to it.";
                case 2:
                    return "A true angler has respect for the water... don't you forget that.";
                case 3:
                    return "Some fish come and go with the seasons. Others only come out at night or in the rain.";
                case 4:
                    return "*mumble* *mumble* ...Eh? I would tell you about my thoughts, but it's a fisherman's secret";
                case 5:
                    return "There are rumors of some very rare fish in these parts... but only an experienced angler could stand a chance against them.";
                case 6:
                    return "If you really want to get the fish biting, make sure you put some bait on your hook.";
                case 7:
                    return "If the local fishing scene got a bit more lively, I might expand the shop's stock.";
                case 8:
                    return "How's the fishing life going for ya? I try to fish as often as possible, but it's not easy when you've got a shop to run.";
                default:
                    return "Hey, it's my skipper! I'd give you a discount on bait if I could afford it.";
            }
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Fish Shop";
            button2 = "Accessory & Bait Shop";
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = FishShopName;
            }
            else
            {
                shop = AccessoryShopName;
            }
        }
        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                {
                    continue;
                }
				// checks if skeletron is down
                if (NPC.downedBoss3)
                {
                    return true;
                }
            }

            return false;
        }

        public override void AddShops()
        {
            var shop = new NPCShop(Type, FishShopName)
                .Add(new Item(ItemID.ArmoredCavefish) { shopCustomPrice = Item.buyPrice(silver: 45) })
                .Add(new Item(ItemID.AtlanticCod) { shopCustomPrice = Item.buyPrice(silver: 22) })
                .Add(new Item(ItemID.Bass) { shopCustomPrice = Item.buyPrice(silver: 15) })
                .Add(new Item(ItemID.CrimsonTigerfish) { shopCustomPrice = Item.buyPrice(silver: 22) })
                .Add(new Item(ItemID.Damselfish) { shopCustomPrice = Item.buyPrice(silver: 90) })
                .Add(new Item(ItemID.DoubleCod) { shopCustomPrice = Item.buyPrice(silver: 45) })
                .Add(new Item(ItemID.Ebonkoi) { shopCustomPrice = Item.buyPrice(silver: 45) })
                .Add(new Item(ItemID.FlarefinKoi) { shopCustomPrice = Item.buyPrice(gold: 1, silver: 50) })
                .Add(new Item(ItemID.Flounder) { shopCustomPrice = Item.buyPrice(silver: 4, copper: 50) })
                .Add(new Item(ItemID.FrostMinnow) { shopCustomPrice = Item.buyPrice(silver: 45) })
                .Add(new Item(ItemID.GoldenCarp) { shopCustomPrice = Item.buyPrice(gold: 30) })
                .Add(new Item(ItemID.Hemopiranha) { shopCustomPrice = Item.buyPrice(silver: 45) })
                .Add(new Item(ItemID.Obsidifish) { shopCustomPrice = Item.buyPrice(silver: 45) })
                .Add(new Item(ItemID.NeonTetra) { shopCustomPrice = Item.buyPrice(silver: 45) })
                .Add(new Item(ItemID.ChaosFish) { shopCustomPrice = Item.buyPrice(gold: 9) }, Condition.Hardmode)
                .Add(new Item(ItemID.PrincessFish) { shopCustomPrice = Item.buyPrice(silver: 75) }, Condition.Hardmode)
                .Add(new Item(ItemID.Prismite) { shopCustomPrice = Item.buyPrice(gold: 3) }, Condition.Hardmode)
                .Add(new Item(ItemID.RedSnapper) { shopCustomPrice = Item.buyPrice(silver: 22, copper: 50) })
                .Add(new Item(ItemID.RockLobster) { shopCustomPrice = Item.buyPrice(silver: 30) })
                .Add(new Item(ItemID.Salmon) { shopCustomPrice = Item.buyPrice(silver: 22, copper: 50) })
                .Add(new Item(ItemID.Shrimp) { shopCustomPrice = Item.buyPrice(silver: 45) })
                .Add(new Item(ItemID.SpecularFish) { shopCustomPrice = Item.buyPrice(silver: 22, copper: 50) })
                .Add(new Item(ItemID.Stinkfish) { shopCustomPrice = Item.buyPrice(silver: 75) })
                .Add(new Item(ItemID.Trout) { shopCustomPrice = Item.buyPrice(silver: 15) })
                .Add(new Item(ItemID.Tuna) { shopCustomPrice = Item.buyPrice(silver: 22, copper: 50) })
                .Add(new Item(ItemID.VariegatedLardfish) { shopCustomPrice = Item.buyPrice(silver: 45) })
                .Add(new Item(ItemID.Honeyfin) { shopCustomPrice = Item.buyPrice(silver: 45) });
            shop.Register();
            shop = new NPCShop(Type, AccessoryShopName)
                .Add(new Item(ItemID.WoodenCrate) { shopCustomPrice = Item.buyPrice(silver: 30) })
                .Add(new Item(ItemID.IronCrate) { shopCustomPrice = Item.buyPrice(gold: 1, silver: 50) })
                .Add(new Item(ItemID.GoldenCrate) { shopCustomPrice = Item.buyPrice(gold: 6) })
                .Add(new Item(ItemID.JungleFishingCrate) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(3206) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.CorruptFishingCrate) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.CrimsonFishingCrate) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.HallowedFishingCrate) { shopCustomPrice = Item.buyPrice(gold: 3) }, Condition.Hardmode)
                .Add(new Item(ItemID.DungeonFishingCrate) { shopCustomPrice = Item.buyPrice(gold: 3) }, Condition.DownedSkeletron)
                .Add(new Item(ItemID.FrozenCrate) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.OasisCrate) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(4877) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.OceanCrate) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.HighTestFishingLine) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.TackleBox) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.AnglerEarring) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.LavaFishingHook) { shopCustomPrice = Item.buyPrice(gold: 6) })
                .Add(new Item(ItemID.FishingBobber) { shopCustomPrice = Item.buyPrice(gold: 1, silver: 50) })
                .Add(new Item(ItemID.AnglerHat) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.AnglerVest) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.AnglerPants) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.FishingPotion) { shopCustomPrice = Item.buyPrice(silver: 6) })
                .Add(new Item(ItemID.Sextant) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.WeatherRadio) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.FishermansGuide) { shopCustomPrice = Item.buyPrice(gold: 3) })
                .Add(new Item(ItemID.SonarPotion) { shopCustomPrice = Item.buyPrice(silver: 6) })
                .Add(new Item(ItemID.CratePotion) { shopCustomPrice = Item.buyPrice(silver: 6) });
            shop.Register();

                        }
    }
}