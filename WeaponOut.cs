using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponOut
{
    public class WeaponOut : Mod
    {
        // public static Mod itemCustomizer;
        public static Mod Instance;

        public override void Load()
        {
            Instance = this;
            On.Terraria.Main.DrawInventory += OnDrawInventory;
            // itemCustomizer = ModLoader.GetMod("ItemCustomizer");
        }

        private void OnDrawInventory(On.Terraria.Main.orig_DrawInventory invoke, Main self)
        {
            invoke(self);
            var texture = Terraria.GameContent.TextureAssets.InventoryTickOn.Value;
            var hoverText = "WeaponOut: " + Lang.inter[59];
            var position = new Vector2(23, 4);
            if (!Main.LocalPlayer.GetModPlayer<WOPlayer>().Show)
            {
                texture = Terraria.GameContent.TextureAssets.InventoryTickOff.Value;
                hoverText = "WeaponOut: " + Lang.inter[60];
            }

            var textureRect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            if (textureRect.Contains(Main.mouseX, Main.mouseY))
            {
                Main.hoverItemName = hoverText;
                Main.blockMouse = true;

                // On click
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    Main.LocalPlayer.GetModPlayer<WOPlayer>().Show = !Main.LocalPlayer.GetModPlayer<WOPlayer>().Show;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModPacket packet = Instance.GetPacket();
                        packet.Write((byte)Main.myPlayer);
                        packet.Write(Main.LocalPlayer.GetModPlayer<WOPlayer>().Show);
                        packet.Send();
                    }
                }
            }

            Main.spriteBatch.Draw(texture, position, null, Color.White);
        }

        public override void Unload()
        {
            On.Terraria.Main.DrawInventory -= OnDrawInventory;
            Instance = null;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            var id = reader.ReadByte();
            var show = reader.ReadBoolean();
            Main.player[id].GetModPlayer<WOPlayer>().Show = show;
            if (Main.dedServ)
            {
                ModPacket packet = Instance.GetPacket();
                packet.Write(id);
                packet.Write(show);
                packet.Send();
            }
        }
    }
}