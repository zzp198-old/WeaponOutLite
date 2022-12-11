using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;

namespace WeaponOut;

public class WOPlayer : ModPlayer
{
    public bool Show = true;

    public override void SaveData(TagCompound tag)
    {
        tag.Add("Show", this.Show);
    }

    public override void LoadData(TagCompound tag)
    {
        if (tag.ContainsKey("Show"))
        {
            Show = tag.GetBool("Show");
        }
    }

    public override void PostUpdate()
    {
        if (Main.netMode == NetmodeID.Server) return; // Oh yeah, server calls this so don't pls

        //change idle pose for player using a heavy weapon
        var heldItem = Player.inventory[Player.selectedItem];
        if (heldItem == null || heldItem.type == 0 || heldItem.holdStyle != 0)
            return; //no item so nothing to show
        Texture2D weaponTex = TextureAssets.Item[heldItem.type].Value;
        if (weaponTex == null) return; //no texture to item so ignore too
        var itemWidth = weaponTex.Width * heldItem.scale;
        var itemHeight = weaponTex.Height * heldItem.scale;
        if (heldItem.ModItem != null)
        {
            if (Main.itemAnimations[heldItem.type] != null)
            {
                itemHeight /= Main.itemAnimations[heldItem.type].FrameCount;
            }
        }

        var larger = Math.Max(itemWidth, itemHeight);
        var playerBodyFrameNum = Player.bodyFrame.Y / Player.bodyFrame.Height;
        if (heldItem.useStyle == 5
            && weaponTex.Width >= weaponTex.Height * 1.2f
            && (!heldItem.noUseGraphic || !heldItem.DamageType.CountsAsClass(DamageClass.Melee))
            && larger >= 45
           )
        {
            if (playerBodyFrameNum == 0) Player.bodyFrame.Y = 10 * Player.bodyFrame.Height;
        }
    }
}