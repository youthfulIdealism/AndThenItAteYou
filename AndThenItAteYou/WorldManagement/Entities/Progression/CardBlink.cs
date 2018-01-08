using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.Input.InputManagers;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Entities.Projectiles;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Progression
{
    public class CardBlink : Card
    {


        public CardBlink(float level) : base(level)
        {
            usable = true;
            cardTex = Game1.texture_card_blink;
            iconTex = Game1.icon_blink;
        }

        public override void upgrade(PlayerBase player, float amt)
        {
            level += amt;
        }

        public override void use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager manager)
        {
            if (manager.click())
            {
                if (charges > 0)
                {
                    SoundManager.getSound("card-use-" + (level - 1)).playWithVariance(0, .5f, 0, SoundType.MONSTER);
                    Vector2 teleportLoc = new Vector2();
                    Vector2 preTeleportLoc = user.location;
                    if (location.X < user.location.X )
                    {
                        teleportLoc = new Vector2(user.location.X - level * 2 * Chunk.tileDrawWidth - Chunk.tileDrawWidth * 3, user.location.Y);
                        TileType teleportTo = world.getBlock(teleportLoc);
                        while (teleportTo != null && teleportTo.tags.Contains(TagReferencer.SOLID))
                        {
                            teleportLoc += new Vector2(Chunk.tileDrawWidth, 0);
                            teleportTo = world.getBlock(teleportLoc);
                        }
                        user.location = teleportLoc;
                        charges--;
                    }else if(location.X > user.location.X)
                    {
                        teleportLoc = new Vector2(user.location.X + level * 2 * Chunk.tileDrawWidth + Chunk.tileDrawWidth * 3, user.location.Y);
                        TileType teleportTo = world.getBlock(teleportLoc);
                        while (teleportTo != null && teleportTo.tags.Contains(TagReferencer.SOLID))
                        {
                            teleportLoc += new Vector2(-Chunk.tileDrawWidth, 0);
                            teleportTo = world.getBlock(teleportLoc);
                        }
                        user.location = teleportLoc;
                        charges--;
                    }

                    for(int i = 0; i < 7; i++)
                    {
                        ParticleArbitrary teleportParticle = new ParticleArbitrary(preTeleportLoc + (teleportLoc - preTeleportLoc) * (float)i / 7, world, new Vector2(), (int)(75 * (1 - 1f / (i + 1))), ((Player)user).texture_run[0]);
                        if (preTeleportLoc.X < teleportLoc.X) { teleportParticle.flip = true; }
                        teleportParticle.gravityMultiplier = 0;
                        teleportParticle.endColor = teleportParticle.endColor * .01f;
                        world.addEntity(teleportParticle);
                    }

                    if(user.inventory.getItemOfType(new Item_Macuhatil(1)) != null)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            ParticleArbitrary teleportParticle = new ParticleArbitrary(preTeleportLoc + (teleportLoc - preTeleportLoc) * (float)i / 7 + new Vector2(0, -10), world, new Vector2(), (int)(75 * (1 - 1f / (i + 1))), Game1.texture_item_macuhatil);
                            teleportParticle.gravityMultiplier = -.1f;
                            teleportParticle.endColor = Color.Red;
                            world.addEntity(teleportParticle);

                            EntityMacuhatilSlash macuhatilSlash = new EntityMacuhatilSlash(preTeleportLoc + (teleportLoc - preTeleportLoc) * (float)i / 7, world, user);
                            macuhatilSlash.velocity += Vector2.Normalize(location - user.location) * .1f;
                            world.addEntity(macuhatilSlash);

                            SoundManager.getSound("sword-slash").playWithVariance(-.5f, .4f, 0, SoundType.MONSTER);
                        }
                    }
                    else if(user.inventory.getItemOfType(new Item_Sword(1)) != null)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            ParticleArbitrary teleportParticle = new ParticleArbitrary(preTeleportLoc + (teleportLoc - preTeleportLoc) * (float)i / 7 + new Vector2(0, -10), world, new Vector2(), (int)(75 * (1 - 1f / (i + 1))), Game1.texture_item_sword);
                            teleportParticle.gravityMultiplier = -.1f;
                            teleportParticle.endColor = Color.Red;
                            world.addEntity(teleportParticle);

                            EntitySwordSlash swordSlash = new EntitySwordSlash(preTeleportLoc + (teleportLoc - preTeleportLoc) * (float)i / 7, world, user);
                            swordSlash.canUnlockWarrior = true;
                            swordSlash.velocity += Vector2.Normalize(location - user.location) * .1f;
                            world.addEntity(swordSlash);

                            SoundManager.getSound("sword-slash").playWithVariance(0f, .4f, 0, SoundType.MONSTER);
                        }
                    }
                }
                else
                {
                    user.speechManager.addSpeechBubble(Game1.texture_item_charmstone);
                }
            }else if(manager.isDown())
            {
                if (charges > 0)
                {
                    Vector2 teleportLoc = new Vector2();
                    Vector2 preTeleportLoc = user.location;
                    if (location.X < user.location.X)
                    {
                        teleportLoc = new Vector2(user.location.X - level * 2 * Chunk.tileDrawWidth - Chunk.tileDrawWidth * 3, user.location.Y);
                        TileType teleportTo = world.getBlock(teleportLoc);
                        while (teleportTo != null && teleportTo.tags.Contains(TagReferencer.SOLID))
                        {
                            teleportLoc += new Vector2(Chunk.tileDrawWidth, 0);
                            teleportTo = world.getBlock(teleportLoc);
                        }
                    }
                    else if (location.X > user.location.X)
                    {
                        teleportLoc = new Vector2(user.location.X + level * 2 * Chunk.tileDrawWidth + Chunk.tileDrawWidth * 3, user.location.Y);
                        TileType teleportTo = world.getBlock(teleportLoc);
                        while (teleportTo != null && teleportTo.tags.Contains(TagReferencer.SOLID))
                        {
                            teleportLoc += new Vector2(-Chunk.tileDrawWidth, 0);
                            teleportTo = world.getBlock(teleportLoc);
                        }
                    }
                    ParticleArbitrary teleportParticle = new ParticleArbitrary(preTeleportLoc + (teleportLoc - preTeleportLoc), world, new Vector2(), 7, ((Player)user).texture_run[0]);
                    if (preTeleportLoc.X < teleportLoc.X) { teleportParticle.flip = true; }
                    teleportParticle.gravityMultiplier = 0;
                    teleportParticle.startColor = Color.Lerp(teleportParticle.startColor, Color.White, .5f);
                    teleportParticle.endColor = teleportParticle.endColor * .01f;
                    world.addEntity(teleportParticle);
                }
                
            }
        }

        public override bool Equals(object obj)
        {
            return obj is CardBlink;
        }
    }
}
