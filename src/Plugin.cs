using AsmResolver.PE.Exports;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using Lunistice_DebugConsole.UI;
using System;
using System.Collections.Generic;
using System.IO;
using Project_Luna;
using Rewired;
using UnityEngine;
using UnityEngine.IO;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.Config;
using UniverseLib.UI;
using Path = Il2CppSystem.IO.Path;
using Player = Project_Luna.Player;

namespace Lunistice_DebugConsole;

public enum Levels
{
    Stage1Act1 = 0,
    Stage1Act2 = 1,
    Stage2Act1 = 2,
    Stage2Act2 = 3,
    Stage3Act1 = 4,
    Stage3Act2 = 5,
    Stage4Act1 = 6,
    Stage4Act2 = 7,
    Stage5Act1 = 8,
    Stage5Act2 = 9,
    Stage6Act1 = 10,
    Stage6Act2 = 11,
    Stage7Act1 = 12,
    Stage7Act2 = 13,
    Stage7ActX = 14,
    Tutorial = 15,
    StageT = 16,
    StageH = 17,
}


[BepInPlugin(GUID, Name, Version)]
public class Plugin : BasePlugin
{
    public const string GUID = "com.GAPLS.Lunistice.DebugConsole";
    public const string Name = "Debug Console";
    public const string Version = "1.0";
    public const string Author = "GAPLS";


    /*
    #region GAME
    public static ConfigEntry<float> TimeScale;
    public static ConfigEntry<float> GravityX;
    public static ConfigEntry<float> GravityY;
    public static ConfigEntry<float> GravityZ;
    #endregion
    */

    // TELEPORT VALUES
    //public static ConfigEntry<List<Transform>> TeleportPoints; //CURRENTLY NOT WORKING (CANT USE TRANSFORMS)

    /*
    #region HANA
    public static ConfigEntry<int> HanaMaxLife;
    public static ConfigEntry<float> HanaSprintSpeed;
    public static ConfigEntry<float> HanaRunSpeed;
    public static ConfigEntry<float> HanaTurboSpeed;
    public static ConfigEntry<float> HanaJumpHeight;
    public static ConfigEntry<float> HanaAttackJump;
    public static ConfigEntry<int> HanaMaxDoubleJumps;
    public static ConfigEntry<float> HanaCoyoteTime;
    public static ConfigEntry<float> HanaFriction;
    public static ConfigEntry<float> HanaAirFriction;
    public static ConfigEntry<float> HanaAcceleration;
    public static ConfigEntry<float> HanaScaleX;
    public static ConfigEntry<float> HanaScaleY;
    public static ConfigEntry<float> HanaScaleZ;
    #endregion

    #region TOREE
    public static ConfigEntry<int> ToreeMaxLife;
    public static ConfigEntry<float> ToreeSprintSpeed;
    public static ConfigEntry<float> ToreeRunSpeed;
    public static ConfigEntry<float> ToreeTurboSpeed;
    public static ConfigEntry<float> ToreeJumpHeight;
    public static ConfigEntry<float> ToreeAttackJump;
    public static ConfigEntry<int> ToreeMaxDoubleJumps;
    public static ConfigEntry<float> ToreeCoyoteTime;
    public static ConfigEntry<float> ToreeFriction;
    public static ConfigEntry<float> ToreeAirFriction;
    public static ConfigEntry<float> ToreeAcceleration;
    public static ConfigEntry<float> ToreeScaleX;
    public static ConfigEntry<float> ToreeScaleY;
    public static ConfigEntry<float> ToreeScaleZ;
    #endregion

    #region TOUKIE
    public static ConfigEntry<int> ToukieMaxLife;
    public static ConfigEntry<float> ToukieSprintSpeed;
    public static ConfigEntry<float> ToukieRunSpeed;
    public static ConfigEntry<float> ToukieTurboSpeed;
    public static ConfigEntry<float> ToukieJumpHeight;
    public static ConfigEntry<float> ToukieAttackJump;
    public static ConfigEntry<int> ToukieMaxDoubleJumps;
    public static ConfigEntry<float> ToukieCoyoteTime;
    public static ConfigEntry<float> ToukieFriction;
    public static ConfigEntry<float> ToukieAirFriction;
    public static ConfigEntry<float> ToukieAcceleration;
    public static ConfigEntry<float> ToukieScaleX;
    public static ConfigEntry<float> ToukieScaleY;
    public static ConfigEntry<float> ToukieScaleZ;
    #endregion
    */
    
    public static ConfigFile gameSettings = new ConfigFile(Path.Combine(Paths.ConfigPath, Name, "Game.cfg"), true);
    public static ConfigFile hanaSettings = new ConfigFile(Path.Combine(Paths.ConfigPath, Name, "Hana.cfg"), true);
    public static ConfigFile toreeSettings = new ConfigFile(Path.Combine(Paths.ConfigPath, Name, "Toree.cfg"), true);
    public static ConfigFile toukieSettings = new ConfigFile(Path.Combine(Paths.ConfigPath, Name, "Toukie.cfg"), true);

    public static ConfigWrapper<float> TimeScale;
    public static ConfigWrapper<float> GravityX;
    public static ConfigWrapper<float> GravityY;
    public static ConfigWrapper<float> GravityZ;
    
    public static ConfigWrapper<int>   HanaMaxLife;
    public static ConfigWrapper<float> HanaSprintSpeed;
    public static ConfigWrapper<float> HanaRunSpeed;
    public static ConfigWrapper<float> HanaTurboSpeed;
    public static ConfigWrapper<float> HanaJumpHeight;
    public static ConfigWrapper<float> HanaAttackJump;
    public static ConfigWrapper<int>   HanaMaxDoubleJumps;
    public static ConfigWrapper<float> HanaCoyoteTime;
    public static ConfigWrapper<float> HanaFriction;
    public static ConfigWrapper<float> HanaAirFriction;
    public static ConfigWrapper<float> HanaAcceleration;
    public static ConfigWrapper<float> HanaScaleX;
    public static ConfigWrapper<float> HanaScaleY;
    public static ConfigWrapper<float> HanaScaleZ;
    
    public static ConfigWrapper<int> ToreeMaxLife;
    public static ConfigWrapper<float> ToreeSprintSpeed;
    public static ConfigWrapper<float> ToreeRunSpeed;
    public static ConfigWrapper<float> ToreeTurboSpeed;
    public static ConfigWrapper<float> ToreeJumpHeight;
    public static ConfigWrapper<float> ToreeAttackJump;
    public static ConfigWrapper<int> ToreeMaxDoubleJumps;
    public static ConfigWrapper<float> ToreeCoyoteTime;
    public static ConfigWrapper<float> ToreeFriction;
    public static ConfigWrapper<float> ToreeAirFriction;
    public static ConfigWrapper<float> ToreeAcceleration;
    public static ConfigWrapper<float> ToreeScaleX;
    public static ConfigWrapper<float> ToreeScaleY;
    public static ConfigWrapper<float> ToreeScaleZ;
    
    public static ConfigWrapper<int> ToukieMaxLife;
    public static ConfigWrapper<float> ToukieSprintSpeed;
    public static ConfigWrapper<float> ToukieRunSpeed;
    public static ConfigWrapper<float> ToukieTurboSpeed;
    public static ConfigWrapper<float> ToukieJumpHeight;
    public static ConfigWrapper<float> ToukieAttackJump;
    public static ConfigWrapper<int> ToukieMaxDoubleJumps;
    public static ConfigWrapper<float> ToukieCoyoteTime;
    public static ConfigWrapper<float> ToukieFriction;
    public static ConfigWrapper<float> ToukieAirFriction;
    public static ConfigWrapper<float> ToukieAcceleration;
    public static ConfigWrapper<float> ToukieScaleX;
    public static ConfigWrapper<float> ToukieScaleY;
    public static ConfigWrapper<float> ToukieScaleZ;
    
    public override void Load()
    {
        // Plugin startup logic
        Log.LogInfo($"Plugin {GUID} is loaded!");

        TimeScale = new ConfigWrapper<float>(gameSettings, "Game", "Time Scale", 1f, "The Time Scale of the Game",
            val => Time.timeScale = val);
        GravityX = new ConfigWrapper<float>(gameSettings, "Game", "Gravity X", 0f, "Gravity on the X Axis",
            val =>
            {
                Player.Gravity =
                    new Vector3(val, Player.Gravity.y, Player.Gravity.z);
                Player.GravityReset = Player.Gravity;
            });
        GravityY = new ConfigWrapper<float>(gameSettings, "Game", "Gravity Y", 14f, "Gravity on the Y Axis",
            val =>
            {
                Player.Gravity =
                    new Vector3(Player.Gravity.x, val, Player.Gravity.z);
                Player.GravityReset = Player.Gravity;
            });
        GravityZ = new ConfigWrapper<float>(gameSettings, "Game", "Gravity Z", 0f, "Gravity on the Z Axis",
            val =>
            {
                Player.Gravity =
                    new Vector3(Player.Gravity.x, Player.Gravity.y, val);
                Player.GravityReset = Player.Gravity;
            });

        #region Hana

        HanaMaxLife = new ConfigWrapper<int>(hanaSettings, "Player", "Max Life", 3, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.MaxLife = val;
        });
        HanaSprintSpeed = new ConfigWrapper<float>(hanaSettings, "Player", "Sprint Speed", 5f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.SprintSpeed = val;
        });
        HanaRunSpeed = new ConfigWrapper<float>(hanaSettings, "Player", "Run Speed", 3f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.RunSpeed = val;
        });
        HanaTurboSpeed = new ConfigWrapper<float>(hanaSettings, "Player", "Turbo Speed", 9f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.TurboSpeed = val;
        });
        //HanaJumpHeight = new ConfigWrapper<float>(hanaSettings, "Hana", "Jump Height", 5f, "", val => Project_Luna.Player.Jump);
        HanaAttackJump = new ConfigWrapper<float>(hanaSettings, "Player", "Attack Jump", 3f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.AttackJumpHeight = val;
        });
        HanaMaxDoubleJumps = new ConfigWrapper<int>(hanaSettings, "Player", "Max Double Jumps", 1, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.MaxDoubleJumps = val;
        });
        HanaCoyoteTime = new ConfigWrapper<float>(hanaSettings, "Player", "Coyote Time", 0.2f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.CoyoteTime = val;
        });
        HanaFriction = new ConfigWrapper<float>(hanaSettings, "Player", "Friction", 20f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.Friction = val;
        });
        HanaAirFriction = new ConfigWrapper<float>(hanaSettings, "Player", "Air Friction", 8f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.AirFriction = val;
        });
        HanaAcceleration = new ConfigWrapper<float>(hanaSettings, "Player", "Acceleration", 14f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.Acceleration = val;
        });
        // HanaScaleX = new ConfigWrapper<float>(hanaSettings, "Player", "Scale X", 1f, "", val => );
        // HanaScaleY = new ConfigWrapper<float>(hanaSettings, "Player", "Scale Y", 1f, "", val => );
        // HanaScaleZ = new ConfigWrapper<float>(hanaSettings, "Player", "Scale Z", 1f, "", val => );#

        #endregion

        #region Toree

        ToreeMaxLife = new ConfigWrapper<int>(toreeSettings, "Player", "Max Life", 3, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.MaxLife = val;
        });
        ToreeSprintSpeed = new ConfigWrapper<float>(toreeSettings, "Player", "Sprint Speed", 5f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.SprintSpeed = val;
        });
        ToreeRunSpeed = new ConfigWrapper<float>(toreeSettings, "Player", "Run Speed", 3f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.RunSpeed = val;
        });
        ToreeTurboSpeed = new ConfigWrapper<float>(toreeSettings, "Player", "Turbo Speed", 9f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.TurboSpeed = val;
        });
        //ToreeJumpHeight = new ConfigWrapper<float>(toreeSettings, "Player", "Jump Height", 5f, "", val => Project_Luna.Player.Jump);
        ToreeAttackJump = new ConfigWrapper<float>(toreeSettings, "Player", "Attack Jump", 3f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.AttackJumpHeight = val;
        });
        ToreeMaxDoubleJumps = new ConfigWrapper<int>(toreeSettings, "Player", "Max Double Jumps", 1, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.MaxDoubleJumps = val;
        });
        ToreeCoyoteTime = new ConfigWrapper<float>(toreeSettings, "Player", "Coyote Time", 0.2f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.CoyoteTime = val;
        });
        ToreeFriction = new ConfigWrapper<float>(toreeSettings, "Player", "Friction", 20f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.Friction = val;
        });
        ToreeAirFriction = new ConfigWrapper<float>(toreeSettings, "Player", "Air Friction", 8f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.AirFriction = val;
        });
        ToreeAcceleration = new ConfigWrapper<float>(toreeSettings, "Player", "Acceleration", 14f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.Acceleration = val;
        });
        // ToreeScaleX = new ConfigWrapper<float>(hanaSettings, "Player", "Scale X", 1f, "", val => );
        // ToreeScaleY = new ConfigWrapper<float>(hanaSettings, "Player", "Scale Y", 1f, "", val => );
        // ToreeScaleZ = new ConfigWrapper<float>(hanaSettings, "Player", "Scale Z", 1f, "", val => );

        #endregion

        #region Toukie

        ToukieMaxLife = new ConfigWrapper<int>(toukieSettings, "Player", "Max Life", 3, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.MaxLife = val;
        });
        ToukieSprintSpeed = new ConfigWrapper<float>(toukieSettings, "Player", "Sprint Speed", 5f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.SprintSpeed = val;
        });
        ToukieRunSpeed = new ConfigWrapper<float>(toukieSettings, "Player", "Run Speed", 3f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.RunSpeed = val;
        });
        ToukieTurboSpeed = new ConfigWrapper<float>(toukieSettings, "Player", "Turbo Speed", 9f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.TurboSpeed = val;
        });
        //ToukieJumpHeight = new ConfigWrapper<float>(toukieSettings, "Player", "Jump Height", 5f, "", val => Project_Luna.Player.Jump);
        ToukieAttackJump = new ConfigWrapper<float>(toukieSettings, "Player", "Attack Jump", 3f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.AttackJumpHeight = val;
        });
        ToukieMaxDoubleJumps = new ConfigWrapper<int>(toukieSettings, "Player", "Max Double Jumps", 1, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.MaxDoubleJumps = val;
        });
        ToukieCoyoteTime = new ConfigWrapper<float>(toukieSettings, "Player", "Coyote Time", 0.2f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.CoyoteTime = val;
        });
        ToukieFriction = new ConfigWrapper<float>(toukieSettings, "Player", "Friction", 20f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.Friction = val;
        });
        ToukieAirFriction = new ConfigWrapper<float>(toukieSettings, "Player", "Air Friction", 8f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.AirFriction = val;
        });
        ToukieAcceleration = new ConfigWrapper<float>(toukieSettings, "Player", "Acceleration", 14f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.Acceleration = val;
        });
        // ToukieScaleX = new ConfigWrapper<float>(toukieSettings, "Player", "Scale X", 1f, "", val => );
        // ToukieScaleY = new ConfigWrapper<float>(toukieSettings, "Player", "Scale Y", 1f, "", val => );
        // ToukieScaleZ = new ConfigWrapper<float>(toukieSettings, "Player", "Scale Z", 1f, "", val => );

        #endregion

        /*
        #region CONFIG SETUP
        TimeScale = Config.Bind("Game", "TimeScale", 1f, "Time Scale of the Game");
        GravityX = Config.Bind("Game.Gravity", "X", 0f, "X Gravity");
        GravityY = Config.Bind("Game.Gravity", "Y", 14f, "Y Gravity");
        GravityZ = Config.Bind("Game.Gravity", "Z", 0f, "Z Gravity");

        HanaMaxLife = Config.Bind("Player.Hana", "MaxLife", 3, "Max Life");
        HanaSprintSpeed = Config.Bind("Player.Hana", "SprintSpeed", 5f, "Sprint Speed");
        HanaRunSpeed = Config.Bind("Player.Hana", "RunSpeed", 3f, "Run Speed");
        HanaTurboSpeed = Config.Bind("Player.Hana", "TurboSpeed", 9f, "Turbo Speed");
        HanaJumpHeight = Config.Bind("Player.Hana", "JumpHeight", 5f, "Jump Height");
        HanaAttackJump = Config.Bind("Player.Hana", "AttackJumpHeight", 3f, "Height gained when doing an attack jump");
        HanaMaxDoubleJumps = Config.Bind("Player.Hana", "MaxDoubleJumps", 1, "Max Double Jumps that can be performed");
        HanaCoyoteTime = Config.Bind("Player.Hana", "CoyoteTime", 0.2f, "Coyote Time");
        HanaFriction = Config.Bind("Player.Hana", "Friction", 20f, "Ground Friction");
        HanaAirFriction = Config.Bind("Player.Hana", "AirFriction", 8f, "Air Friction");
        HanaAcceleration = Config.Bind("Player.Hana", "Acceleration", 14f, "Acceleration");
        HanaScaleX = Config.Bind("Player.Hana.Scale", "Scale X", 1f, "Scale X");
        HanaScaleY = Config.Bind("Player.Hana.Scale", "Scale Y", 1f, "Scale Y");
        HanaScaleZ = Config.Bind("Player.Hana.Scale", "Scale Z", 1f, "Scale Z");

        ToreeMaxLife = Config.Bind("Player.Toree", "MaxLife", 1, "Max Life");
        ToreeSprintSpeed = Config.Bind("Player.Toree", "SprintSpeed", 6f, "Sprint Speed");
        ToreeRunSpeed = Config.Bind("Player.Toree", "RunSpeed", 4f, "Run Speed");
        ToreeTurboSpeed = Config.Bind("Player.Toree", "TurboSpeed", 10f, "Turbo Speed");
        ToreeJumpHeight = Config.Bind("Player.Toree", "JumpHeight", 5f, "Jump Height");
        ToreeAttackJump = Config.Bind("Player.Toree", "AttackJumpHeight", 3f, "Height gained when doing an attack jump");
        ToreeMaxDoubleJumps = Config.Bind("Player.Toree", "MaxDoubleJumps", 1, "Max Double Jumps that can be performed");
        ToreeCoyoteTime = Config.Bind("Player.Toree", "CoyoteTime", 0.2f, "Coyote Time");
        ToreeFriction = Config.Bind("Player.Toree", "Friction", 20f, "Ground Friction");
        ToreeAirFriction = Config.Bind("Player.Toree", "AirFriction", 8f, "Air Friction");
        ToreeAcceleration = Config.Bind("Player.Toree", "Acceleration", 18f, "Acceleration");
        ToreeScaleX = Config.Bind("Player.Toree.Scale", "ScaleX", 1f, "Scale X");
        ToreeScaleY = Config.Bind("Player.Toree.Scale", "ScaleY", 1f, "Scale Y");
        ToreeScaleZ = Config.Bind("Player.Toree.Scale", "ScaleZ", 1f, "Scale Z");

        ToukieMaxLife = Config.Bind("Player.Toukie", "MaxLife", 2, "Max Life");
        ToukieSprintSpeed = Config.Bind("Player.Toukie", "SprintSpeed", 5f, "Sprint Speed");
        ToukieRunSpeed = Config.Bind("Player.Toukie", "RunSpeed", 3f, "Run Speed");
        ToukieTurboSpeed = Config.Bind("Player.Toukie", "TurboSpeed", 9f, "Turbo Speed");
        ToukieJumpHeight = Config.Bind("Player.Toukie", "JumpHeight", 4f, "Jump Height");
        ToukieAttackJump = Config.Bind("Player.Toukie", "AttackJumpHeight", 2f, "Height gained when doing an attack jump");
        ToukieMaxDoubleJumps = Config.Bind("Player.Toukie", "MaxDoubleJumps", 3, "Max Double Jumps that can be performed");
        ToukieCoyoteTime = Config.Bind("Player.Toukie", "CoyoteTime", 0.2f, "Coyote Time");
        ToukieFriction = Config.Bind("Player.Toukie", "Friction", 20f, "Ground Friction");
        ToukieAirFriction = Config.Bind("Player.Toukie", "Air Friction", 6f, "Air Friction");
        ToukieAcceleration = Config.Bind("Player.Toukie", "Acceleration", 14f, "Acceleration");
        ToukieScaleX = Config.Bind("Player.Toukie.Scale", "ScaleX", 1f, "Scale X");
        ToukieScaleY = Config.Bind("Player.Toukie.Scale", "ScaleY", 1f, "Scale Y");
        ToukieScaleZ = Config.Bind("Player.Toukie.Scale", "ScaleZ", 1f, "Scale Z");
        #endregion
        */

        float startupDelay = 1f;
        UniverseLibConfig config = new()
        {
            Disable_EventSystem_Override = false,
            Force_Unlock_Mouse = true,
            Unhollowed_Modules_Folder = @"D:\Steam\steamapps\common\Lunistice\BepInEx\interop"
        };

        Universe.Init(startupDelay, OnInitialised, LogHandler, config);
        
    }

    public static UIBase UiBase { get;private set; }
    public static DebugMenuUI HanaMyPanel;
    void OnInitialised()
    {
        UiBase = UniversalUI.RegisterUI("MyPanel", UiUpdate);
        HanaMyPanel = new(UiBase);
    }

    void UiUpdate()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            UniverseLib.Config.ConfigManager.Force_Unlock_Mouse = !UniverseLib.Config.ConfigManager.Force_Unlock_Mouse;
            HanaMyPanel.SetActive(!HanaMyPanel.Enabled);
        }
    }

    void LogHandler(string message, LogType type)
    {

    }
}