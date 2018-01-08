using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Survive.Input;
using Survive.Input.Data;
using Survive.Sound;
using Survive.SplashScreens;
using Survive.Worldgen;
using Survive.WorldManagement;
using Survive.WorldManagement.ContentProcessors;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Progression;
using Survive.WorldManagement.Entities.TransformedPlayers;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.IO;
using Survive.WorldManagement.Procedurals;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.ExtensionTileTypes;
using Survive.WorldManagement.Tile.Tags;
using Survive.WorldManagement.Worlds.Cutscenes;
using System;
using System.Collections.Generic;

namespace Survive
{
    public class Game1 : Game
    {
        public static Game1 instance { get; private set; }
        public static RasterizerState scizzorRasterState;
        public static bool paused { get; set; }
        public static bool isInTutorial { get; set; }
        public static KeyBindManager keyBindManager;
        public static int selectedSaveSlot = 0;

        public GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public List<SplashScreen> queuedSplashScreens;

        private static Texture2D linetex;
        public static Texture2D block;
        public static Texture2D[] cloud;
        public static Texture2D guitar;
        public static Texture2D controlHelp;
        public static Texture2D texture_sky;
        public static Texture2D sheetMusic;
        public static Texture2D childsDrawing;
        public static Texture2D finds_sister;
        public static Texture2D ContinueButton;
        public static Texture2D OptionButton;
        public static Texture2D SaveAndExitButton;
        public static Texture2D SuicideButton;
        public static Texture2D abilityUIElement;
        public static Texture2D icon_food;
        public static Texture2D icon_health;
        public static Texture2D icon_warmth;
        public static Texture2D icon_explosion;
        public static Texture2D icon_blink;
        public static Texture2D icon_immunity;
        public static Texture2D icon_sticky;
        public static Texture2D icon_jump;
        public static Texture2D icon_speed;
        public static Texture2D icon_regen;
        public static Texture2D icon_girl;
        public static Texture2D arrow;
        public static Texture2D fog;
        public static Texture2D rain;
        public static Texture2D sunbeam;
        public static Texture2D SpeechBubble;
        public static Texture2D UIInventory_Arrow;
        public static Texture2D UIInventory_Edge;
        public static Texture2D UIInventory_ItemHolder;
        public static Texture2D UIInventory_StandardCorner;
        public static Texture2D UIInventory_TopLeftCorner;
        public static Texture2D UIInventory_BuildButton;
        public static Texture2D UIInventory_KeyedItem;
        public static Texture2D KeyboardTex;
        public static Texture2D backpackTex;
        public static Texture2D handTex;
        /*public Texture2D UIMainMenu_L;
        public Texture2D UIMainMenu_C;
        public Texture2D UIMainMenu_R;*/
        public static Texture2D UIMainMenu_tutorial;
        public static Texture2D UIMainMenu_new;
        public static Texture2D UIMainMenu_continue;
        public static Texture2D Skull;
        public static Texture2D Darkness;
        public static Texture2D charLock;
        public static Texture2D unlock_huntress;
        public static Texture2D unlock_shaman;
        public static Texture2D unlock_girl;
        public static Texture2D unlock_warrior;
        public static Texture2D downArrow;
        public static Texture2D texture_x;
        public static Texture2D cursor;
        public static Texture2D ui_bar_start;
        public static Texture2D ui_bar_middle;
        public static Texture2D ui_bar_end;
        public static Texture2D ui_bar_start_back;
        public static Texture2D ui_bar_middle_back;
        public static Texture2D ui_bar_end_back;

        public WorldBase world;

        

        //public List<Texture2D> terrainImages;
        public List<Texture2D> primitives;
        public static Texture2D texture_item_grass;
        public static Texture2D texture_item_clock;
        public static Texture2D texture_item_rope;
        public static Texture2D texture_mushroom;
        public static Texture2D texture_mushroom_poisoned;
        public static Texture2D texture_bow;
        public static Texture2D texture_stick;
        public static Texture2D texture_item_stone;
        public static Texture2D texture_item_knife;
        public static Texture2D texture_item_spear;
        public static Texture2D texture_item_meat;
        public static Texture2D texture_item_feather;
        public static Texture2D texture_item_fire;
        public static Texture2D texture_item_grappling_hook;
        public static Texture2D texture_item_laser_gun;
        public static Texture2D texture_item_bullet;
        public static Texture2D texture_item_spud;
        public static Texture2D texture_item_spade;
        public static Texture2D texture_item_spear_fanged;
        public static Texture2D texture_item_guardian_fang;
        public static Texture2D[] texture_berries;
        public static Texture2D[] texture_potions;
        public static Texture2D texture_item_sword;
        public static Texture2D texture_item_macuhatil;
        public static Texture2D texture_item_axe;
        public static Texture2D texture_item_scizors;
        public static Texture2D texture_item_childs_drawing;
        public static Texture2D texture_item_totem_blank;
        public static Texture2D texture_item_totem_quetzoatl;
        public static Texture2D texture_item_totem_condor;
        public static Texture2D texture_item_totem_falcon;
        public static Texture2D texture_item_totem_rabbit;
        public static Texture2D texture_item_totem_tapir;
        public static Texture2D texture_item_totem_crocodile;
        public static Texture2D texture_item_whirlwind;
        public static Texture2D texture_item_charmstone;
        public static Texture2D texture_item_ladder;
        public static Texture2D texture_item_snare;
        public static Texture2D texture_item_pickaxe;
        public static Texture2D texture_item_harpoon;
        public static Texture2D texture_item_net;
        public static Texture2D texture_item_bite;
        public static Texture2D texture_item_seed;

        public static Texture2D texture_sun;
        public static Texture2D texture_checkmark;
        public static Texture2D texture_crossout;

        public static PlayerAnimationPackage player_default_animations;
        public static PlayerAnimationPackage player_hunter_animations;
        public static PlayerAnimationPackage player_shaman_animations;
        public static PlayerAnimationPackage player_girl_animations;
        public static PlayerAnimationPackage player_warrior_animations;
        public static Texture2D texture_entity_moose_stand;
        public static Texture2D[] texture_entity_moose_run;
        public static Texture2D texture_entity_turkey_stand;
        public static Texture2D[] texture_entity_turkey_fly;
        public static Texture2D texture_entity_arrow;
        public static Texture2D texture_entity_spear;
        public static Texture2D texture_entity_bomb;
        public static Texture2D texture_entity_bomb_sticky;
        public static Texture2D texture_entity_harpoon;
        public static Texture2D[] texture_entity_fire;
        public static Texture2D texture_entity_fire_glow;
        public static Texture2D texture_entity_fire_mask;
        public static Texture2D texture_entity_grappling_hook;
        public static Texture2D texture_entity_rope;
        public static Texture2D texture_entity_remains;
        public static Texture2D[] texture_entity_guardian_stand;
        public static Texture2D[] texture_entity_guardian_run;
        public static Texture2D[] texture_entity_laser_bolt;
        public static Texture2D[] texture_entity_antlion_stand;
        public static Texture2D texture_entity_constable_tower;
        public static Texture2D texture_entity_constable_eye;
        public static Texture2D texture_entity_girl_stand;
        public static Texture2D[] texture_entity_girl_run;
        public static Texture2D[] texture_entity_frog_stand;
        public static Texture2D texture_entity_frog_jump;
        public static Texture2D texture_entity_frog_attack;
        public static Texture2D texture_entity_frog_spit;
        public static Texture2D[] texture_entity_wheelie_stand;
        public static Texture2D[] texture_entity_wheelie_run;
        public static Texture2D[] texture_entity_wheelie_charge_jump;
        public static Texture2D[] texture_entity_owl_stand;
        public static Texture2D texture_entity_owl_attack;
        public static Texture2D texture_entity_owl_retreat;
        public static SoundEffect[,] celloNotes;
        public static Texture2D texture_entity_jellyfish_body;
        public static Texture2D texture_entity_jellyfish_tentacles;
        public static Texture2D texture_particle_bubble;
        public static Texture2D[] texture_quetzoatl;
        public static Texture2D texture_rabbit;
        public static Texture2D[] texture_rabbit_run;
        public static Texture2D[] texture_tapir;
        public static Texture2D texture_dad;
        public static Texture2D texture_mom;
        public static Texture2D texture_tent;
        public static Texture2D[] texture_entity_blob_jump;
        public static Texture2D[] texture_entity_blob_attack;
        public static Texture2D texture_entity_snare;
        public static Texture2D[] texture_entity_croc_stand;
        public static Texture2D[] texture_entity_croc_attack;
        public static Texture2D[] texture_entity_croc_walk;
        public static Texture2D[] texture_entity_cricket;
        public static Texture2D[] texture_entity_butterfly;
        public static Texture2D[] texture_entity_bat;
        public static Texture2D[] texture_monsterpede_head;
        public static Texture2D[] texture_monsterpede_body;
        public static Texture2D[] texture_monsterpede_butt;
        public static Texture2D texture_fish;
        public static Texture2D[] texture_condor;
        public static Texture2D[] texture_condor_flap;
        public static Texture2D[] texture_falcon;
        public static Texture2D[] texture_falcon_flap;
        public static Texture2D texture_worm_body;
        public static Texture2D[] texture_worm_head;

        public static Texture2D texture_particle_glow;
        public static Texture2D texture_particle_frostbreath;
        public static Texture2D texture_particle_jump;
        public static Texture2D texture_particle_speed;
        public static Texture2D texture_particle_blood;
        public static Texture2D texture_particle_whirlwind;

        public static Texture2D texture_card_speed;
        public static Texture2D texture_card_jump;
        public static Texture2D texture_card_maxhealth;
        public static Texture2D texture_card_regen;
        public static Texture2D texture_card_explosive;
        public static Texture2D texture_card_immunity;
        public static Texture2D texture_card_blink;
        public static Texture2D texture_card_sticky;

        public static SpriteFont defaultFont;
        public static SpriteFont gamefont_12;
        public static SpriteFont gamefont_24;
        public static SpriteFont gamefont_32;
        public static SpriteFont gamefont_72;

        public static KeyboardState lastKeyboardState;
        public static MouseState lastMouseState;

        protected bool hasLoaded;
        public CraftingDictionary crafting;

        float blackoutRemaining = 2f;

        public const int findGirlWorld = 8;
        public const int victoryWorld = 9;
        protected bool startWithMainMenu = true;

        public void refreshGraphicSettings()
        {
            graphics.PreferredBackBufferWidth = MetaData.screenW;
            graphics.PreferredBackBufferHeight = MetaData.screenH;

            switch (MetaData.screenSetting)
            {
                case 0:
                    break;
                case 1:
                    Window.IsBorderless = true;
                    break;
                case 2:
                    graphics.IsFullScreen = true;
                    break;
                default:
                    Logger.log("Invalid screen setting: " + MetaData.screenSetting + ". Defaulting to borderless fullscreen.");
                    Window.IsBorderless = true;
                    break;
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            instance = this;
            
        }

        protected override void Initialize()
        {
            scizzorRasterState = new RasterizerState() { ScissorTestEnable = true };
            queuedSplashScreens = new List<SplashScreen>();
            if (startWithMainMenu) { queuedSplashScreens.Add(new MainMenu()); }

            lastKeyboardState = Keyboard.GetState();
            lastMouseState = Mouse.GetState();
            

            KeyManagerEnumerator.reBuildStringAssociations();
            keyBindManager = new KeyBindManager();
            GameSaverAndLoader.loadKeyBinds(0);

            SoundManager.Initialize();

            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            block = Content.Load<Texture2D>("Block");
            controlHelp = Content.Load<Texture2D>("help");
            texture_sun = Content.Load<Texture2D>("sun");
            //cloud = Content.Load<Texture2D>("cloudBase");
            cloud = Content.loadTextureRange("cloud_", 4);
            sunbeam = Content.Load<Texture2D>("Sunbeam");
            texture_sky = Content.Load<Texture2D>("NightSky");
            guitar = Content.Load<Texture2D>("guitar");
            sheetMusic = Content.Load<Texture2D>("sheetMusic");
            childsDrawing = Content.Load<Texture2D>("child_drawing");
            finds_sister = Content.Load<Texture2D>("finds_sister");
            ContinueButton = Content.Load<Texture2D>("UI/ContinueButton");
            SaveAndExitButton = Content.Load<Texture2D>("UI/SaveAndExitButton");
            SuicideButton = Content.Load<Texture2D>("UI/SuicideButton");
            OptionButton = Content.Load<Texture2D>("UI/OptionsButton");
            arrow = Content.Load<Texture2D>("arrow");
            fog = Content.Load<Texture2D>("Particles/fog");
            rain = Content.Load<Texture2D>("rain");
            SpeechBubble = Content.Load<Texture2D>("UI/SpeechBubble");
            UIInventory_Arrow = Content.Load<Texture2D>("UI/Arrow");
            UIInventory_Edge = Content.Load<Texture2D>("UI/Edge");
            UIInventory_ItemHolder = Content.Load<Texture2D>("UI/ItemHolder");
            UIInventory_StandardCorner = Content.Load<Texture2D>("UI/StandardCorner");
            UIInventory_TopLeftCorner = Content.Load<Texture2D>("UI/TopLeftCorner");
            UIInventory_BuildButton = Content.Load<Texture2D>("UI/BuildButton");
            UIInventory_KeyedItem = Content.Load<Texture2D>("UI/KeyedItem");
            KeyboardTex = Content.Load<Texture2D>("Keyboard");
            backpackTex = Content.Load<Texture2D>("UI/backpack");
            handTex = Content.Load<Texture2D>("UI/hand");
            UIMainMenu_tutorial = Content.Load<Texture2D>("UI/button_tutorial");
            UIMainMenu_new = Content.Load<Texture2D>("UI/button_newgame");
            UIMainMenu_continue = Content.Load<Texture2D>("UI/button_continue");
            Skull = Content.Load<Texture2D>("UI/Skull");
            charLock = Content.Load<Texture2D>("lock");
            unlock_huntress = Content.Load<Texture2D>("Entities/Players/hunter_unlock");
            unlock_shaman = Content.Load<Texture2D>("Entities/Players/shaman_unlock");
            unlock_girl = Content.Load<Texture2D>("Entities/Players/girl_unlock");
            unlock_warrior = Content.Load<Texture2D>("Entities/Players/warrior_unlock");
            Darkness = Content.Load<Texture2D>("darkness_mask");
            downArrow = Content.Load<Texture2D>("UI/DownArrow");
            texture_x = Content.Load<Texture2D>("UI/X");
            cursor = Content.Load<Texture2D>("UI/Cursor");
            ui_bar_start = Content.Load<Texture2D>("UI/Bar-Start");
            ui_bar_middle = Content.Load<Texture2D>("UI/Bar-Middle");
            ui_bar_end = Content.Load<Texture2D>("UI/Bar-End");
            ui_bar_start_back = Content.Load<Texture2D>("UI/Bar-Start-Back");
            ui_bar_middle_back = Content.Load<Texture2D>("UI/Bar-Middle-Back");
            ui_bar_end_back = Content.Load<Texture2D>("UI/Bar-End-Back");


            defaultFont = Content.Load<SpriteFont>("Temp_0");
            gamefont_12 = Content.Load<SpriteFont>("GameLang_12");
            gamefont_24 = Content.Load<SpriteFont>("GameLang_24");
            gamefont_32 = Content.Load<SpriteFont>("GameLang_32");
            gamefont_72 = Content.Load<SpriteFont>("GameLang_72");

            loadEntityAssets();
            loadParticleAssets();
            loadTerrainAssets();
            loadItemAssets();
            loadSounds();
            loadDecorations();

            linetex = new Texture2D(GraphicsDevice, 1, 1);
            linetex.SetData<Color>(new Color[] { Color.White });

            
            

            


            


            texture_card_speed = Content.Load<Texture2D>("Cards/card_run");
            texture_card_jump = Content.Load<Texture2D>("Cards/card_jump");
            texture_card_maxhealth = Content.Load<Texture2D>("Cards/card_maxhealth");
            texture_card_regen = Content.Load<Texture2D>("Cards/card_regen");
            texture_card_explosive = Content.Load<Texture2D>("Cards/card_explosive");
            texture_card_immunity = Content.Load<Texture2D>("Cards/card_immunity");
            texture_card_blink = Content.Load<Texture2D>("Cards/card_blink");
            texture_card_sticky = Content.Load<Texture2D>("Cards/card_sticky");

            texture_checkmark = Content.Load<Texture2D>("UI/checkmark");
            texture_crossout = Content.Load<Texture2D>("UI/crossout");
            abilityUIElement = Content.Load<Texture2D>("UI/abilityMarker");
            icon_food = Content.Load<Texture2D>("UI/icon_food");
            icon_health = Content.Load<Texture2D>("UI/icon_health");
            icon_warmth = Content.Load<Texture2D>("UI/icon_warmth");
            icon_explosion = Content.Load<Texture2D>("UI/icon_explosion");
            icon_blink = Content.Load<Texture2D>("UI/icon_blink");
            icon_immunity = Content.Load<Texture2D>("UI/icon_immunity");
            icon_sticky = Content.Load<Texture2D>("UI/icon_sticky");
            icon_jump = Content.Load<Texture2D>("UI/icon_jumpBoost");
            icon_regen = Content.Load<Texture2D>("UI/icon_regen");
            icon_speed = Content.Load<Texture2D>("UI/icon_speedBoost");
            icon_girl = Content.Load<Texture2D>("UI/icon_girl");
            hasLoaded = true;

            
            
        }

        protected virtual void loadEntityAssets()
        {
            player_default_animations = new PlayerAnimationPackage(Game1.instance.Content.Load<Texture2D>("Entities/Players/Default/stand_0"),
                Content.loadTextureRange("Entities/Players/Default/run_", 7),
                Content.loadTextureRange("Entities/Players/Default/jump_", 1),
                Content.loadTextureRange("Entities/Players/Default/throw_", 8),
                Content.loadTextureRange("Entities/Players/Default/swing_", 4));

            player_hunter_animations = new PlayerAnimationPackage(Game1.instance.Content.Load<Texture2D>("Entities/Players/Hunter/stand_0"),
                Content.loadTextureRange("Entities/Players/Hunter/run_", 7),
                Content.loadTextureRange("Entities/Players/Hunter/jump_", 1),
                Content.loadTextureRange("Entities/Players/Hunter/throw_", 7),
                Content.loadTextureRange("Entities/Players/Hunter/swing_", 4));

            player_shaman_animations = new PlayerAnimationPackage(Game1.instance.Content.Load<Texture2D>("Entities/Players/Shaman/stand_0"),
                Content.loadTextureRange("Entities/Players/Shaman/run_", 7),
                Content.loadTextureRange("Entities/Players/Shaman/jump_", 1),
                Content.loadTextureRange("Entities/Players/Shaman/throw_", 6),
                Content.loadTextureRange("Entities/Players/Shaman/swing_", 4));

            player_girl_animations = new PlayerAnimationPackage(Game1.instance.Content.Load<Texture2D>("Entities/Girl/stand_0"),
                Content.loadTextureRange("Entities/Girl/run_", 7),
                new Texture2D[] { Content.Load<Texture2D>("Entities/Girl/run_2") },
                    Content.loadTextureRange("Entities/Girl/throw_", 5),
                    Content.loadTextureRange("Entities/Girl/swing_", 2));

            player_warrior_animations = new PlayerAnimationPackage(Game1.instance.Content.Load<Texture2D>("Entities/Players/Warrior/stand_0"),
                Content.loadTextureRange("Entities/Players/Warrior/run_", 7),
                Content.loadTextureRange("Entities/Players/Warrior/jump_", 1),
                Content.loadTextureRange("Entities/Players/Warrior/throw_", 6),
                Content.loadTextureRange("Entities/Players/Warrior/swing_", 4));

            texture_entity_fire = Content.loadTextureRange("Entities/Fire/", 6);

            texture_entity_guardian_stand = Content.loadTextureRange("Entities/guardian/guardian_stand_", 2);
            texture_entity_guardian_run = Content.loadTextureRange("Entities/guardian/run_", 4);

            texture_entity_laser_bolt = Content.loadTextureRange("Entities/energyBolt/", 5);

            texture_entity_constable_tower = Content.Load<Texture2D>("Entities/Constable/tower_0");
            texture_entity_constable_eye = Content.Load<Texture2D>("Entities/Constable/eye_0");

            texture_entity_frog_stand = Content.loadTextureRange("Entities/frog/stand_", 2);
            texture_entity_frog_jump = Content.Load<Texture2D>("Entities/frog/jump_0");
            texture_entity_frog_attack = Content.Load<Texture2D>("Entities/frog/attack_0");
            texture_entity_frog_spit = Content.Load<Texture2D>("Entities/entity_frog_spit");

            texture_entity_wheelie_stand = Content.loadTextureRange("Entities/Wheelie/stand_", 1);
            texture_entity_wheelie_run = Content.loadTextureRange("Entities/Wheelie/run_", 2);
            texture_entity_wheelie_charge_jump = Content.loadTextureRange("Entities/Wheelie/jump_", 2);

            texture_entity_antlion_stand = Content.loadTextureRange("Entities/Antlion/Stand_", 3);

            texture_entity_owl_stand = Content.loadTextureRange("Entities/Owl/stand_", 2);
            texture_entity_owl_attack = Content.Load<Texture2D>("Entities/Owl/attack_0");
            texture_entity_owl_retreat = Content.Load<Texture2D>("Entities/Owl/retreat_0");

            texture_entity_jellyfish_body = Content.Load<Texture2D>("Entities/jellyfish/body");
            texture_entity_jellyfish_tentacles = Content.Load<Texture2D>("Entities/jellyfish/tentacles");

            texture_entity_moose_stand = Content.Load<Texture2D>("Entities/Moose/stand_0");
            texture_entity_moose_run = Content.loadTextureRange("Entities/Moose/run_", 4);

            texture_entity_turkey_stand = Content.Load<Texture2D>("Entities/Turkey/stand_0");
            texture_entity_turkey_fly = Content.loadTextureRange("Entities/Turkey/fly_", 3);

            texture_entity_arrow = Content.Load<Texture2D>("Entities/entity_arrow");
            texture_entity_spear = Content.Load<Texture2D>("Entities/entity_spear");

            texture_entity_fire_glow = Content.Load<Texture2D>("Entities/Fire/glow");
            texture_entity_fire_mask = Content.Load<Texture2D>("FireCover");

            texture_entity_grappling_hook = Content.Load<Texture2D>("Entities/entity_grappling_hook");
            texture_entity_rope = Content.Load<Texture2D>("Entities/entity_rope");
            texture_entity_remains = Content.Load<Texture2D>("Entities/remains");

            texture_entity_girl_stand = Content.Load<Texture2D>("Entities/Girl/stand_0");
            texture_entity_girl_run = Content.loadTextureRange("Entities/Girl/run_", 7);

            texture_quetzoatl = Content.loadTextureRange("Entities/Quetzalcoatl/Quetzalcoatl_", 2);
            texture_rabbit = Content.Load<Texture2D>("Entities/Rabbit/rabbit_stand");
            texture_rabbit_run = Content.loadTextureRange("Entities/Rabbit/rabbit_", 3);
            texture_tapir = Content.loadTextureRange("Entities/Taiper/taipir_", 5);

            texture_dad = Content.Load<Texture2D>("Entities/Family/Dad/stand_0");
            texture_mom = Content.Load<Texture2D>("Entities/Family/Mom/stand_0");
            texture_tent = Content.Load<Texture2D>("Entities/Family/Tent/Tent");

            texture_entity_snare = Content.Load<Texture2D>("entity_snare");
            //texture_entity_blob_jump = Content.loadTextureRange("Entities/Blob/blob_", 2);
            //texture_entity_blob_attack = Content.loadTextureRange("Entities/Blob/blob_", 2);

            texture_entity_croc_stand = Content.loadTextureRange("Entities/Crocodile/croc_", 0);
            texture_entity_croc_attack = Content.loadTextureRange("Entities/Crocodile/croc_attack_", 1);
            texture_entity_croc_walk = Content.loadTextureRange("Entities/Crocodile/croc_walk_", 7);

            texture_entity_cricket = Content.loadTextureRange("Entities/Decorative/cricket_", 2);
            texture_entity_butterfly = Content.loadTextureRange("Entities/Decorative/butterfly_", 2);
            texture_entity_bat = Content.loadTextureRange("Entities/Decorative/bat_", 3);

            texture_entity_harpoon = Content.Load<Texture2D>("Entities/entity_harpoon");

            texture_monsterpede_head = Content.loadTextureRange("Entities/MonsterPede/monsterpedeHead_", 2);
            texture_monsterpede_body = Content.loadTextureRange("Entities/MonsterPede/monsterpedeLegs_", 3);
            texture_monsterpede_butt = Content.loadTextureRange("Entities/MonsterPede/monsterpedeButt_", 2);

            texture_entity_bomb = Content.Load<Texture2D>("Entities/entity_bomb");
            texture_entity_bomb_sticky = Content.Load<Texture2D>("Entities/entity_bomb_sticky");

            texture_fish = Content.Load<Texture2D>("Entities/Fish/fish_0");

            texture_condor = Content.loadTextureRange("Entities/Condor/condor-", 3);
            texture_condor_flap = Content.loadTextureRange("Entities/Condor/condor-flap-", 5);

            texture_falcon = Content.loadTextureRange("Entities/Falcon/falcon_", 3);
            texture_falcon_flap = Content.loadTextureRange("Entities/Falcon/falcon_flap_", 6);

            texture_worm_body = Content.Load<Texture2D>("Entities/Worm/body_0");
            texture_worm_head = Content.loadTextureRange("Entities/Worm/head_", 2);
        }

        protected virtual void loadParticleAssets()
        {
            texture_particle_glow = Content.Load<Texture2D>("Particles/glow");
            texture_particle_frostbreath = Content.Load<Texture2D>("Particles/frostbreath");
            texture_particle_jump = Content.Load<Texture2D>("Particles/jump");
            texture_particle_speed = Content.Load<Texture2D>("Particles/dust");
            texture_particle_blood = Content.Load<Texture2D>("Particles/blood");
            texture_particle_whirlwind = Content.Load<Texture2D>("Particles/whirlwind");
            texture_particle_bubble = Content.Load<Texture2D>("Particles/bubble");
        }

        protected virtual void loadTerrainAssets()
        {
            primitives = new List<Texture2D>();
            for (int i = 0; i <= 17; i++)
            {
                primitives.Add(Content.Load<Texture2D>("Blocks/Primitives/" + i));
            }

            TileTypeReferencer.Load(Content);

            ItemDropper dropGrass = new ItemDropper();
            dropGrass.registerNewDrop(new Item_Grass(0), null, 3, .7f);
            ItemDropper dropRope = new ItemDropper();
            dropRope.registerNewDrop(new Item_Rope(0), null, 1, 1);
            ItemDropper dropShrooms = new ItemDropper();
            dropShrooms.registerNewDrop(new Item_Mushroom(0), null, 2, .5f);
            ItemDropper dropSticks = new ItemDropper();
            dropSticks.registerNewDrop(new Item_Stick(0), null, 1, .1f);
            ItemDropper dropStones = new ItemDropper();
            dropStones.registerNewDrop(new Item_Stone(0), null, 3, .33f);
            ItemDropper dropCharmstoneGarunteed = new ItemDropper();
            dropCharmstoneGarunteed.registerNewDrop(new Item_Charmstone(0), null, 1, 1);
            ItemDropper dropCharmstoneExtra = new ItemDropper();
            dropCharmstoneExtra.registerNewDrop(new Item_Charmstone(0), null, 1, .5f);
            ItemDropper dropAltarItems = new ItemDropper();
            dropAltarItems.registerNewDrop(new Item_Rope(0), null, 5, .75f);
            dropAltarItems.registerNewDrop(new Item_Meat(0), null, 1, .1f);

            HarvestDictionary.registerNewHarvest(TileTypeReferencer.MUSHROOMS, new ItemDropper[] { dropShrooms });
            HarvestDictionary.registerNewHarvest(TileTypeReferencer.STONES, new ItemDropper[] { dropStones });
            HarvestDictionary.registerNewHarvest(TileTypeReferencer.ALTAR, new ItemDropper[] { dropAltarItems });
            ItemDropper dropTreasuresRarely = new ItemDropper();
        }

        protected override void OnExiting(Object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            SoundManager.UnLoad();

            Logger.flush();
        }

        protected virtual void loadItemAssets()
        {
            texture_item_grass = Content.Load<Texture2D>("Items/item_grass");
            texture_item_clock = Content.Load<Texture2D>("Items/clock");
            texture_item_rope = Content.Load<Texture2D>("Items/item_rope");
            texture_mushroom = Content.Load<Texture2D>("Items/item_mushroom_poison");
            texture_mushroom_poisoned = Content.Load<Texture2D>("Items/item_mushroom");
            texture_stick = Content.Load<Texture2D>("Items/item_stick");
            texture_bow = Content.Load<Texture2D>("Items/item_bow");
            texture_item_stone = Content.Load<Texture2D>("Items/item_stone");
            texture_item_knife = Content.Load<Texture2D>("Items/item_knife");
            texture_item_spear = Content.Load<Texture2D>("Items/item_spear");
            texture_item_meat = Content.Load<Texture2D>("Items/item_meat");
            texture_item_feather = Content.Load<Texture2D>("Items/item_feather");
            texture_item_fire = Content.Load<Texture2D>("Items/item_fire");
            texture_item_grappling_hook = Content.Load<Texture2D>("Items/item_grappling_hook");
            texture_item_laser_gun = Content.Load<Texture2D>("Items/item_energyWeapon");
            texture_item_bullet = Content.Load<Texture2D>("Items/item_bullet");
            //texture_item_berry = Content.Load<Texture2D>("Items/item_berry");
            texture_berries = Content.loadTextureRange("Items/item_berry_", 3);
            texture_potions = Content.loadTextureRange("Items/bottle_", 3);
            texture_item_spud = Content.Load<Texture2D>("Items/item_spud");
            texture_item_spade = Content.Load<Texture2D>("Items/item_spade");
            texture_item_spear_fanged = Content.Load<Texture2D>("Items/item_spear_fanged");
            texture_item_guardian_fang = Content.Load<Texture2D>("Items/item_guardian_tooth");
            texture_item_sword = Content.Load<Texture2D>("Items/item_sword");
            texture_item_axe = Content.Load<Texture2D>("Items/item_axe");
            texture_item_macuhatil = Content.Load<Texture2D>("Items/item_macuhatil");
            texture_item_scizors = Content.Load<Texture2D>("Items/item_scizors");
            texture_item_childs_drawing = Content.Load<Texture2D>("Items/item_childs_drawing");
            texture_item_snare = Content.Load<Texture2D>("Items/item_snare");
            texture_item_pickaxe = Content.Load<Texture2D>("Items/item_pickaxe");
            texture_item_harpoon = Content.Load<Texture2D>("Items/item_harpoon");
            texture_item_net = Content.Load<Texture2D>("Items/item_net");


            texture_item_totem_blank = Content.Load<Texture2D>("Items/totem");
            texture_item_totem_quetzoatl = Content.Load<Texture2D>("Items/totemQuetz");
            texture_item_totem_rabbit = Content.Load<Texture2D>("Items/item_totemRabbit");
            texture_item_totem_tapir = Content.Load<Texture2D>("Items/item_totemTapir");
            texture_item_totem_crocodile = Content.Load<Texture2D>("Items/item_totemcrocodile");
            texture_item_totem_condor = Content.Load<Texture2D>("Items/item_totemcondor");
            texture_item_totem_falcon = Content.Load<Texture2D>("Items/item_totemfalcon");

            texture_item_whirlwind = Content.Load<Texture2D>("Items/whirlwind");
            texture_item_charmstone = Content.Load<Texture2D>("Items/item_charmstone");

            texture_item_ladder = Content.Load<Texture2D>("Items/item_ladder");
            texture_item_bite = Content.Load<Texture2D>("Items/item_bite");
            texture_item_seed = Content.Load<Texture2D>("Items/item_seed");


            crafting = new CraftingDictionary();
            crafting.registerRecepie(new CraftingRecepie(new Item_Bow(100), .4f, new Item[] { new Item_Stick(1), new Item_Rope(1) }, new int[] { 1, 2 }));
            crafting.registerRecepie(new CraftingRecepie(new Item_Arrow(3), .3f, new Item[] { new Item_Feather(1), new Item_Stick(1), new Item_Rope(1), new Item_Stone(1) }, new int[] { 1, 1, 1, 1 }));
            crafting.registerRecepie(new CraftingRecepie(new Item_Rope(1), .1f, new Item[] { new Item_Grass(1) }, new int[] { 2 }));
            crafting.registerRecepie(new CraftingRecepie(new Item_Rope(5), .1f * 5, new Item[] { new Item_Grass(1) }, new int[] { 2 * 5 }));
            //crafting.registerRecepie(new CraftingRecepie(new Item_Knife(30), new Item[] { new Item_Stick(1), new Item_Rope(1), new Item_Stone(1) }, new int[] { 1, 1, 1 }));
            crafting.registerRecepie(new CraftingRecepie(new Item_Spear(1), .2f, new Item[] { new Item_Stick(1), new Item_Rope(1), new Item_Stone(1) }, new int[] { 1, 1, 1 }));
            crafting.registerRecepie(new CraftingRecepie(new Item_Fire(1), .05f, new Item[] { new Item_Stick(1) }, new int[] { 2 }));
            crafting.registerRecepie(new CraftingRecepie(new Item_Fire(1), .05f, new Item[] { new Item_Grass(1), new Item_Stone(1) }, new int[] { 3, 2 }));
            crafting.registerRecepie(new CraftingRecepie(new Item_Snare(1), .05f, new Item[] { new Item_Rope(1) }, new int[] { 3 }));
        }

        protected virtual void loadSounds()
        {
            celloNotes = new SoundEffect[3, 7];
            for (int i = 0; i < 3; i++)
            {
                for (int z = 0; z < 7; z++)
                {
                    celloNotes[i, z] = Content.Load<SoundEffect>("Sounds/Cello/" + i + "" + z);
                }
            }

            SoundManager.Load(Content);
        }

        protected virtual void loadDecorations()
        {
            
        }

        public Texture2D[] loadTextureRange(String prefix, int amount)
        {
            Texture2D[] receptacle = new Texture2D[amount + 1];
            for (int i = 0; i <= amount; i++)
            {
                receptacle[i] = Content.Load<Texture2D>(prefix + i);
            }
            return receptacle;
        }

        protected override void UnloadContent()
        {
            try
            {
                SoundManager.UnLoad();
                System.Threading.Thread.Sleep(500);
            }
            catch(Exception exception)
            {
                Logger.log(exception.ToString());
            }
            Logger.flush();

            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Delete))
            {
                GameSaverAndLoader.save(Game1.selectedSaveSlot, world);
                Exit();
            }


            

            if (hasLoaded && !paused)
            {
                SoundManager.update();

                if(queuedSplashScreens.Count <= 0)
                {
                    blackoutRemaining *= .99f;
                    blackoutRemaining -= .001f;

                    /*if (world == null)
                    {
                        GameSaverAndLoader.load(0);
                        //setUpNewWorld();
                        
                    }
                    else
                    {*/
                        world.update(gameTime);
                    //}
                }
                else
                {
                    queuedSplashScreens[0].acceptInput(gameTime, Keyboard.GetState(), Mouse.GetState(), lastKeyboardState, lastMouseState);
                    if(queuedSplashScreens[0].canContinue())
                    {
                        queuedSplashScreens.RemoveAt(0);
                    }
                }
                
            }

            if(Logger.logTracker > 5)//TODO: experiment with values
            {
                Logger.flush();
            }

            lastKeyboardState = Keyboard.GetState();
            lastMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        public void switchWorlds(WorldBase switchTo)
        {
            


            if (world == null)
            {
                world = switchTo;
                blackoutRemaining = 2f;
                ((Player)(world.player)).hasLeveledUpThisWorld = false;
                world.switchTo();
            }
            else
            {
                
                Entity playerBase = world.player;
                if (playerBase is TransformedPlayer)
                {
                    while (!(playerBase is Player))
                    {
                        playerBase = ((TransformedPlayer)playerBase).transformedFrom;
                    }
                }
                Player player = (Player)playerBase;

                switchTo.entities.Remove(switchTo.player);
                world.player.location = switchTo.player.location;
                switchTo.timeOfDay = world.timeOfDay;
                player.location = switchTo.player.location;
                player.velocity = new Vector2();
                player.impulse = new Vector2();
                switchTo.player = player;
                switchTo.entities.Add(player);
                switchTo.player.world = switchTo;
                world.Dispose();
                world = switchTo;
                blackoutRemaining = 2f;
                ((Player)world.player).hasLeveledUpThisWorld = false;
                world.switchTo();
            }
        }

        public void returnToMainMenu()
        {
            world.Dispose();
            this.world = null;
            queuedSplashScreens.Add(new MainMenu());
        }

        /**
            use teleporter to move to next location
        */
        public void progress(int oldDifficulty)
        {
            int newDifficulty = oldDifficulty + 1;


            WorldBase newWorld = getWorldBasedOnDifficulty(newDifficulty);

            if(newWorld == null)
            {
                newWorld = new World("" + new Random(System.DateTime.Now.Millisecond).NextDouble() + "" + new Random(System.DateTime.Now.Minute).NextDouble() + "" + new Random((int)System.DateTime.Now.Ticks).NextDouble() + "" + new Random().NextDouble(), newDifficulty);
            }

            switchWorlds(newWorld);

            //TODO: move to getWorldBasedOnDifficulty
            /*if (newDifficulty == findGirlWorld)
            {
                EntityGirl girl = new EntityGirl(new Vector2(100, ((World)world).noise.octavePerlin1D(100) * ((FindGirlCutscene)world).decorator.getTerrainMultiplier() * Chunk.tileDrawWidth), world);
                world.addEntity(girl);
            }*/

            

        }

        public WorldBase getWorldBasedOnDifficulty(int difficulty)
        {
            /*if (World.universeProperties == null)
            {
                World.universeProperties = new UniverseProperties("" + new Random(System.DateTime.Now.Millisecond).NextDouble() + "" + new Random(System.DateTime.Now.Minute).NextDouble() + "" + new Random((int)System.DateTime.Now.Ticks).NextDouble() + "" + new Random().NextDouble());
            }*/

            Console.WriteLine("difficultty: " + difficulty);
            Console.WriteLine("adaptive difficulty: " + MetaData.difficultyModifier);
           /* if (difficulty == 0 && UniverseProperties.unlocks.Count <= 1)
            {
                Dictionary<Rectangle, string> images = new Dictionary<Rectangle, string>();

                List<String> challengeImages = UniverseProperties.challangeListToStringList();

                int imageWidth = 1000;
                int imageHeight = 400;
                for (int i = 0; i < 6; i++)
                {
                    images.Add(new Rectangle(imageWidth * i, Game1.instance.graphics.PreferredBackBufferHeight / 2 - imageHeight / 2, imageWidth, imageHeight), "StorySplashes/story_" + i);
                }

                Game1.instance.queuedSplashScreens.Add(new ScrollingSplashScreen(images, new List<string>()));
            }*/
            /*if (difficulty == 1)
            {
                WorldFromDisk newWorld = new WorldFromDisk("Content\\Worlds\\advertisingWorld", difficulty);
                
                newWorld.player.location = new Vector2(0, -100);
                return newWorld;
            }*/
            if(difficulty == findGirlWorld)
            {
                World nextWorld = new World(Math.Pow(5 + difficulty, 3) * UniverseProperties.seed.GetHashCode() + "", difficulty);
                FindGirlCutscene cutsceneWorld = new FindGirlCutscene(nextWorld);
                return cutsceneWorld;
            }
            if (difficulty == victoryWorld)
            {
                /* WorldFromDisk newWorld = new WorldFromDisk("Content\\Worlds\\lastWorld", difficulty);
                 EntityGirl sister = new EntityGirl(new Vector2(0, -100), newWorld);
                 sister.touchedPlayer = true;
                 (newWorld).queuedEntites.Add(sister);
                 EntityFamily family = new EntityFamily(new Vector2(700, -100), newWorld);
                 (newWorld).queuedEntites.Add(family);
                 newWorld.player.location = new Vector2(0, -100);
                 return newWorld;*/

                World nextWorld = new World(Math.Pow(5 + difficulty, 3) * UniverseProperties.seed.GetHashCode() + "", difficulty);
                CreditsCutscene cutsceneWorld = new CreditsCutscene(nextWorld);
                return cutsceneWorld;
            }
            
            return new World(Math.Pow(5 + difficulty, 3) * UniverseProperties.seed.GetHashCode() + "", difficulty);
        }

        public void handlePlayerDeath()
        {
            if (world != null)
            {
                if (world.difficulty < MetaData.prevDifficultyReached)
                {
                    MetaData.difficultyModifier = Math.Max(-1, MetaData.difficultyModifier - 1);
                }
                else if (world.difficulty > MetaData.prevDifficultyReached)
                {
                    MetaData.difficultyModifier++;
                }
                MetaData.prevDifficultyReached = world.difficulty;

                Console.WriteLine("Difficulty Modifier: " + MetaData.difficultyModifier);
                Console.WriteLine("Previously Reached Difficulty: " + MetaData.prevDifficultyReached);

                GameSaverAndLoader.saveMeta(Game1.selectedSaveSlot);
            }

            Logger.flush();

            DeathScreen deathscreen = new DeathScreen();
            deathscreen.switchTo();
            queuedSplashScreens.Add(deathscreen);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);



            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, scizzorRasterState);
            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            if (queuedSplashScreens.Count >= 1 && queuedSplashScreens[0].isBlockingDrawCalls())
            {
                queuedSplashScreens[0].draw(spriteBatch);
            }
            else
            {
                if (hasLoaded)
                {
                    world.draw(spriteBatch, gameTime);
                }

                if (blackoutRemaining > 0)
                {
                    spriteBatch.Draw(block, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.Black * blackoutRemaining);
                    //spriteBatch.Draw(controlHelp, new Rectangle(graphics.PreferredBackBufferWidth / 2 - controlHelp.Width / 2, graphics.PreferredBackBufferHeight / 2 - controlHelp.Height / 2, controlHelp.Width, controlHelp.Height), Color.White * blackoutRemaining);
                }


                if (queuedSplashScreens.Count >= 1)
                {
                    queuedSplashScreens[0].draw(spriteBatch);
                }

                /*if (Keyboard.GetState().IsKeyDown(Keys.Tab))
                {
                    spriteBatch.Draw(controlHelp, new Rectangle(graphics.PreferredBackBufferWidth / 2 - controlHelp.Width / 2, graphics.PreferredBackBufferHeight / 2 - controlHelp.Height / 2, controlHelp.Width, controlHelp.Height), Color.White);
                }*/
            }



            /*if (paused)
            {
                spriteBatch.Draw(block, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.Black * .5f);
            }*/

            Point mouseloc = Mouse.GetState().Position;
            spriteBatch.Draw(cursor, new Rectangle(mouseloc.X - cursor.Width / 2, mouseloc.Y - cursor.Height / 2, cursor.Width, cursor.Height), Color.White);

            spriteBatch.End();

            

            base.Draw(gameTime);
        }

        //TODO: replace
        public static string decimalToBase6(long decimalNumber)
        {
            const int BitsInLong = 64;
            const string Digits = "0123456";

            if (decimalNumber == 0)
                return "0";

            int index = BitsInLong - 1;
            long currentNumber = Math.Abs(decimalNumber);
            char[] charArray = new char[BitsInLong];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % 6);
                charArray[index--] = Digits[remainder];
                currentNumber = currentNumber / 6;
            }

            string result = new String(charArray, index + 1, BitsInLong - index - 1);
            if (decimalNumber < 0)
            {
                result = "-" + result;
            }

            return result;
        }

        /**
            Utility to draw a line.
         //TODO: move into a "graphics helper" class
        */
        public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, int width, Color color)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle = (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(linetex,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    width), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }

        /**
            Draw a line with default settings
            //TODO: move into a "graphics helper" class
        */
        public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color)
        {
            DrawLine(sb, start, end, 4, color);

        }
    }
}
