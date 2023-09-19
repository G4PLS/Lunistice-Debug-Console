using AsmResolver.PE.Exports;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.IO;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Lunistice_DebugConsole;

public enum Levels
{
    Stage1_Act1 = 0,
    Stage1_Act2 = 1,
    Stage2_Act1 = 2,
    Stage2_Act2 = 3,
    Stage3_Act1 = 4,
    Stage3_Act2 = 5,
    Stage4_Act1 = 6,
    Stage4_Act2 = 7,
    Stage5_Act1 = 8,
    Stage5_Act2 = 9,
    Stage6_Act1 = 10,
    Stage6_Act2 = 11,
    Stage7_Act1 = 12,
    Stage7_Act2 = 13,
    Stage7_ActX = 14,
    Tutorial = 15,
    StageT = 16,
    StageH = 17,
}


[BepInPlugin(GUID, NAME, VERSION)]
public class Plugin : BasePlugin
{
    public const string GUID = "com.GAPLS.Lunistice.DebugConsole";
    public const string NAME = "Debug Console";
    public const string VERSION = "1.0";
    public const string AUTHOR = "GAPLS";


    #region GAME
    public static ConfigEntry<float> TimeScale;
    public static ConfigEntry<float> GravityX;
    public static ConfigEntry<float> GravityY;
    public static ConfigEntry<float> GravityZ;
    #endregion

    // TELEPORT VALUES
    //public static ConfigEntry<List<Transform>> TeleportPoints; //CURRENTLY NOT WORKING (CANT USE TRANSFORMS)

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

    public override void Load()
    {
        // Plugin startup logic
        Log.LogInfo($"Plugin {GUID} is loaded!");

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


        var debugConsole = IL2CPPChainloader.AddUnityComponent(typeof(DebugConsole));

        var harmony = new Harmony("Lunistice.DebugConsole");
        harmony.PatchAll(typeof(DebugConsole));
        harmony.PatchAll(typeof(Plugin));
    }
}